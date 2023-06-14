using System.Collections;
using UnityEngine;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Skills.LiveSkill;
using UniRx;
using Assets.Scripts.Events;

namespace Assets.Scripts.Skills.MoveSkill
{
    public class MoveSkillBehavior : SkillBehavior
    {
        public Speed speed;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private float _velocity;
        private Vector3 _movement;
        private Vector3 _input;
        private LiveSkillBehavior _liveScript;
        private VariableJoystick _joystick;
        [SerializeField]
        private bool _isIdle = false;
        private CompositeDisposable _disposables = new CompositeDisposable();

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();

            _joystick = GameManager.Instance.joystick;
            speed = new Speed();
            var selectedSkillEventStream = MessageBroker.Default.Receive<SelectedSkillEvent>().Subscribe(isIdle => { SetIdle(true); });
            var playingEventStream = MessageBroker.Default.Receive<PlayingEvent>().Subscribe(isIdle => { SetIdle(false); });

            selectedSkillEventStream.AddTo(_disposables);
            playingEventStream.AddTo(_disposables);
        }
        private void Start()
        {
            _liveScript = GetComponent<LiveSkillBehavior>();
        }
        void FixedUpdate()
        {
            if (!_liveScript.health.IsDead)
            {
                MovementHandler();
                _velocity = _joystick.Horizontal != 0 || _joystick.Vertical != 0 ? 1 : 0;
                SetAnimation(_velocity);
                if (_velocity != 0)
                {
                    Rotation();
                }
            }
        }
        void SetIdle(bool state)
        {
            //Debug.Log("SetIdle : " + state);
            _isIdle = state;
            _animator.SetFloat("Velocity", 0);
            _animator.SetBool("IsIdle", true);
        }
        void MovementHandler()
        {
            _input = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
            _movement = _input;
            _movement.Normalize();

            if (_movement == Vector3.zero)
            {
                return;
            }

            if (!_liveScript.health.IsDead)
                _rigidbody.MovePosition(transform.position + _movement * speed.RealPoint * Time.fixedDeltaTime);

        }
        void Rotation()
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movement);
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(targetRotation);
        }

        private void SetAnimation(float input)
        {
            if (_animator != null)
            {
                _animator.SetFloat("Velocity", input);
                if (input == 0)
                {
                    _animator.SetBool("IsIdle", true);
                }
                else
                {
                    _animator.SetBool("IsIdle", false);
                }
            }
        }
        private void OnDestroy()
        {
            _disposables.Clear();
        }
    }
}