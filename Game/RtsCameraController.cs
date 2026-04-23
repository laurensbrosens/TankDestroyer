using Godot;

namespace TankDestroyer;

public partial class RtsCameraController : Node3D
{
	[ExportGroup("Movement")] [Export] public bool AllowMovement { get; set; } = true;
	[ExportGroup("Movement")] [Export] public bool Freeze { get; set; } = false;

	[ExportGroup("Movement")]
	[Export(PropertyHint.Range, "0,150,")]
	public float MovementSpeed { get; set; }

	[ExportGroup("Movement")]
	[Export(PropertyHint.Range, "0,150,0.1")]
	public float DeadZoneMouse { get; set; }

	[ExportGroup("Movement")]
	[ExportSubgroup("Rotation")]
	[Export(PropertyHint.Range, "0,1000,0.1")]
	public float RotationSpeed { get; set; }

	[ExportGroup("Movement")]
	[ExportSubgroup("Rotation")]
	[Export(PropertyHint.Range, "0,1000,0.1")]
	public float MouseRotationSpeed { get; set; }

	[ExportGroup("Movement")]
	[ExportSubgroup("Rotation")]
	[Export(PropertyHint.Range, "0,90,")]
	public float MinElevationAngle { get; set; }

	[ExportGroup("Movement")]
	[ExportSubgroup("Rotation")]
	[Export(PropertyHint.Range, "0,90,")]
	public float MaxElevationAngle { get; set; }

	[ExportGroup("Movement")]
	[ExportSubgroup("Rotation")]
	[Export]
	public bool AllowRotation { get; set; } = true;

	[ExportGroup("Movement")]
	[ExportSubgroup("Rotation")]
	[Export]
	public bool InvertY { get; set; } = false;


	[ExportGroup("Movement")]
	[ExportSubgroup("Zoom")]
	[Export]
	public bool AllowZoom { get; set; } = true;

	[ExportGroup("Movement")]
	[ExportSubgroup("Zoom")]
	[Export]
	public bool ZoomToCursor { get; set; } = false;

	[ExportGroup("Movement")]
	[ExportSubgroup("Zoom")]
	[Export(PropertyHint.Range, "0,100,0.1")]
	public float MinZoom { get; set; } = 10;

	[ExportGroup("Movement")]
	[ExportSubgroup("Zoom")]
	[Export(PropertyHint.Range, "0,100,0.1")]
	public float MaxZoom { get; set; } = 90;

	[ExportGroup("Movement")]
	[ExportSubgroup("Zoom")]
	[Export(PropertyHint.Range, "0,100,0.1")]
	public float ZoomSpeed { get; set; } = 10f;

	[ExportGroup("Movement")]
	[ExportSubgroup("Zoom")]
	[Export(PropertyHint.Range, "0,1,0.1")]
	public float ZoomDampening { get; set; } = 0.5f;

	[ExportGroup("Movement")]
	[ExportSubgroup("Pan")]
	[Export]
	public bool AllowPan { get; set; } = true;

	[ExportGroup("Movement")]
	[ExportSubgroup("Pan")]
	[Export(PropertyHint.Range, "0,10,0.1")]
	public float PanSpeed { get; set; } = 2f;

	[Export] private Node3D _elevationNode;
	[Export] private Camera3D _cameraNode;

	private bool _forwardPressed;
	private bool _backwardPressed;
	private bool _leftPressed;
	private bool _rightPressed;

	private bool _rotateLeft;
	private bool _rotateRight;
	private bool _isRotationWithMouse;
	private bool _isPanningWithMouse;

	private Vector2 _lastMousePosition;
	private Vector2 _displacement;
	private float _zoomDirection;
	private Vector3 _velocity;

	public override void _Process(double delta)
	{
		if (Freeze)
		{
			return;
		}

		if (AllowMovement)
		{
			MoveCameraWithKeys(delta /  Godot.Engine.TimeScale);
			MoveCameraWithMouse(delta /  Godot.Engine.TimeScale);
		}

		if (AllowRotation)
		{
			RotateCameraFromKeys(delta /  Godot.Engine.TimeScale);
			RotateCameraFromMouse(delta /  Godot.Engine.TimeScale);
		}

		if (AllowZoom)
		{
			DoZoom(delta /  Godot.Engine.TimeScale);
		}
	}

	private Tween _lastJumpTween;

	public void JumpToPosition(Vector3 location, float duration = 1f)
	{
		Freeze = true;
		_lastJumpTween?.Stop();
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", location, 1.0f);
		tween.SetEase(Tween.EaseType.Out);
		tween.Finished += EndJump;
		tween.Play();

		_lastJumpTween = tween;
	}

	public void EndJump()
	{
		_lastJumpTween?.Stop();
		Freeze = false;
	}

	private void MoveCameraWithMouse(double delta)
	{
		if (!_isPanningWithMouse || _displacement.Length().EqualsWithMargin(0f))
		{
			return;
		}

		var velocity = _displacement * PanSpeed;
		var newPosition = Transform.Origin - (Transform.Basis * velocity.ToVector3() * (float)delta);
		var transform = Transform;
		transform.Origin = newPosition;
		Transform = transform;
	}

	private void DoZoom(double delta)
	{
		if (_zoomDirection == 0)
		{
			return;
		}

		var newZoom = _cameraNode.Position.Z + _zoomDirection * ZoomSpeed * delta;
		newZoom = Mathf.Clamp(newZoom, MinZoom, MaxZoom);

		var position = _cameraNode.Position;
		position.Z = (float)newZoom;
		_cameraNode.Position = position;

		_zoomDirection *= ZoomDampening;
		if (Mathf.Abs(_zoomDirection) < 0.001f)
		{
			_zoomDirection = 0f;
		}
	}

	private void RotateCameraFromMouse(double delta)
	{
		if (!_isRotationWithMouse)
		{
			return;
		}

		RotateLeftRightFromMouse(delta);
		ChangeElevationFromMouse(delta);
	}

	private void ChangeElevationFromMouse(double delta)
	{
		var newElevation = _elevationNode.RotationDegrees.X +
						   (_displacement.Y * (InvertY ? -1f : 1f)) * MouseRotationSpeed * (float)delta;
		newElevation = Mathf.Clamp(newElevation, -MaxElevationAngle, -MinElevationAngle);

		var rotation = _elevationNode.RotationDegrees;
		rotation.X = newElevation;
		_elevationNode.RotationDegrees = rotation;
	}

	private void RotateLeftRightFromMouse(double delta)
	{
		var rotation = RotationDegrees;
		rotation.Y += _displacement.X * MouseRotationSpeed * (float)delta;
		RotationDegrees = rotation;
	}


	private void RotateCameraFromKeys(double delta)
	{
		if (_rotateLeft)
		{
			var rotation = RotationDegrees;
			rotation.Y -= RotationSpeed * (float)delta;
			RotationDegrees = rotation;
		}

		if (_rotateRight)
		{
			var rotation = RotationDegrees;
			rotation.Y += RotationSpeed * (float)delta;
			RotationDegrees = rotation;
		}
	}

	private void MoveCameraWithKeys(double delta)
	{
		_velocity = CalculateVelocity();

		var newPosition = Transform.Origin + (_velocity.Normalized() * MovementSpeed * (float)delta);

		var transform = Transform;
		transform.Origin = newPosition;
		Transform = transform;
	}

	private Vector3 CalculateVelocity()
	{
		var velocity = new Vector3();
		if (_forwardPressed)
		{
			velocity -= Transform.Basis.Z;
		}

		if (_backwardPressed)
		{
			velocity += Transform.Basis.Z;
		}

		if (_leftPressed)
		{
			velocity -= Transform.Basis.X;
		}

		if (_rightPressed)
		{
			velocity += Transform.Basis.X;
		}

		return velocity;
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		_forwardPressed = IsPressed(inputEvent, "camera_forward", _forwardPressed);
		_backwardPressed = IsPressed(inputEvent, "camera_backward", _backwardPressed);
		_leftPressed = IsPressed(inputEvent, "camera_left", _leftPressed);
		_rightPressed = IsPressed(inputEvent, "camera_right", _rightPressed);

		_rotateLeft = IsPressed(inputEvent, "camera_rotate_left", _rotateLeft);
		_rotateRight = IsPressed(inputEvent, "camera_rotate_right", _rotateRight);

		_isRotationWithMouse = IsPressed(inputEvent, "camera_rotate_mouse_enabled", _isRotationWithMouse);
		_isPanningWithMouse = IsPressed(inputEvent, "camera_pan_mouse_enabled", _isPanningWithMouse);

		if (inputEvent.IsActionPressed("camera_zoom_in"))
		{
			_zoomDirection = -1;
		}

		if (inputEvent.IsActionPressed("camera_zoom_out"))
		{
			_zoomDirection = 1;
		}

		CalculateMouseDisplacement();
	}

	private void CalculateMouseDisplacement()
	{
		var mousePosition = GetViewport().GetMousePosition();
		_displacement = mousePosition - _lastMousePosition;
		if (_displacement.Length() < DeadZoneMouse)
		{
			_displacement = Vector2.Zero;
		}

		_lastMousePosition = mousePosition;
	}

	public bool IsPressed(InputEvent inputEvent, string action, bool currentValue)
	{
		return !inputEvent.IsActionReleased(action) && (currentValue || inputEvent.IsActionPressed(action));
	}
}
