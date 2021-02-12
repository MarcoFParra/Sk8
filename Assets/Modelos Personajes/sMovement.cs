using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sMovement : MonoBehaviour
{
    private Animator anim;
    public CharacterController controller;
    public Transform cam;
    private AudioSource audioSource;
    public AudioClip song;


    public float speed = 1.5f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }
    bool isplaying = false;
    float horizontal = 0f;
    float vertical = 0f;

    string m_ClipName;
    AnimatorClipInfo[] m_CurrentClipInfo;
    bool isRunning = false;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRunning = !isRunning;
        }
        m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
        m_ClipName = m_CurrentClipInfo[0].clip.name;
        //Debug.Log("Animacion actual:" + m_ClipName);
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (isRunning)
        {
            horizontal = horizontal * 2;
            vertical = vertical * 2;
        }
        anim.SetFloat("velX", horizontal);
        anim.SetFloat("velY", vertical);

        //Debug.Log("velX " + horizontal + " VelY " + vertical);


        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Debug.Log("------Tarjet Angulo de rotacion-------" + targetAngle);
            Debug.Log("------Tarjet Angulo de rotacion camara -------" + cam.eulerAngles.y);

            float angle = Mathf.SmoothDampAngle(controller.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Debug.Log("------Angulo de rotacion-------" + angle);

            controller.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            

            if (m_ClipName.Equals("Run"))
            {
                if (!isplaying)
                {
                    Debug.Log("------Audio Play-------");
                    audioSource.Play();
                    isplaying = true;
                }
            }
            else
            {
                if (isplaying)
                {
                    Debug.Log("------Audio Pause-------");
                    isplaying = false;
                    audioSource.Stop();
                }
            }
            if (m_ClipName.Equals("Stop_Walking") || m_ClipName.Equals("Start_Walking") || m_ClipName.Equals("idle"))
                return;



            //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}