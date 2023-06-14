using Assets.Scripts.Skills.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float timimg;
    public GameObject dialogPanel;
    public GameObject guidePanel;
    public Joystick joystick;
    public GameObject skillHolder;
    public GameObject novaSkill;
    public Speed speed;
    [SerializeField]
    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _velocity;
    private Vector3 _movement;
    private Vector3 _input;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        skillHolder.SetActive(false);
        StartCoroutine(MoveHeroInPos(Vector3.zero));
    }

    public void TogglePausing(bool isPause)
    {
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            StartCoroutine(TapToContinues());
        }
    }

    ///<summary> wait for user's input action(joy stick, tap) to continues game </summary>
    IEnumerator TapToContinues()
    {
        guidePanel.SetActive(true);
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            //Debug.Log("Wait for key down");

            if (joystick.Horizontal != 0 || joystick.Vertical != 0 || Input.anyKey)
            {
                break;
            }
        }
        guidePanel.SetActive(false);
        skillHolder.SetActive(true);
        Time.timeScale = 1;
    }
    public void ResetPositionHero()
    {
        StartCoroutine(GotoDefaultPos(Vector3.zero));
    }
    IEnumerator GotoDefaultPos(Vector3 pos)
    {
        //Debug.Log("AHHHHHH");
        skillHolder.gameObject.SetActive(false);
        joystick.gameObject.SetActive(false);
        while (Vector3.Distance(transform.position, pos) > 0.2f)
        {
            SetAnimation(1);
            var direction = pos - transform.position;
            direction.Normalize();
            _rigidbody.MovePosition(transform.position + direction * speed.RealPoint * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(targetRotation);
            yield return new WaitForFixedUpdate();
        }
        //panel.SetActive(true);
        EnemiesBehaviourController.Instance.BossesCircleFormation();
        _velocity = 0;
        SetAnimation(0);
    }
    IEnumerator MoveHeroInPos(Vector3 pos)
    {
        joystick.gameObject.SetActive(false);
        while (transform.position.z < pos.z)
        {
            SetAnimation(1);
            _rigidbody.MovePosition(transform.position + Vector3.forward * speed.RealPoint * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        dialogPanel.SetActive(true);
        joystick.gameObject.SetActive(true);
        TogglePausing(true);
        StartCoroutine(InputControll());
    }

    public void OnHeroDeadCutscene()
    {
        animator.SetBool("IsDead", true);
        Invoke(nameof(EnablePanelDialog), 2.0f);
    }

    void EnablePanelDialog()
    {
        dialogPanel.SetActive(true);
        //Time.timeScale = 0;
    }

    IEnumerator InputControll()
    {
        while (joystick.isActiveAndEnabled)
        {
            MovementHandler();
            _velocity = joystick.Horizontal != 0 || joystick.Vertical != 0 ? 1 : 0;
            SetAnimation(_velocity);
            skillHolder.transform.position = this.transform.position;
            if (_velocity != 0)
            {
                Rotation();
            }
            yield return null;
        }
    }

    void SetIdle(bool state)
    {
        _animator.SetFloat("Velocity", 0);
        _animator.SetBool("IsIdle", true);
    }
    void MovementHandler()
    {
        _input = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        _movement = _input;
        _movement.Normalize();

        if (_movement == Vector3.zero)
        {
            return;
        }

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
}
