using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueQueue : MonoBehaviour {

    [System.Serializable]
    public class TextMessage
    {
        public TextMessage(string message, float time)
        {
            Message = message;
            Time = time;
        }
        public string Message { get; set; }
        public float Time { get; set; }
    }

    [SerializeField]
    private Image textBoxRoot;
    [SerializeField]
    private Text text;

    private Queue<TextMessage> messageQueue;
    private float timer = 0;
    private float cooldownTimer = 0;
    private bool showingMessages = false;

    private AudioSource audioSource;

	void Start () {
        messageQueue = new Queue<TextMessage>();
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
        if (showingMessages)
        {
            ProcessQueue();
        }
    }

    void ProcessQueue()
    {
        if (textBoxRoot.gameObject.activeInHierarchy)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                textBoxRoot.gameObject.SetActive(false);
                if (messageQueue.Count > 0)
                {
                    DisplayNextMessage();
                }
                else
                {
                    showingMessages = false;
                }
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                textBoxRoot.gameObject.SetActive(true);
            }
        }
    }

    public static DialogueQueue Get()
    {
        return GameObject.Find("Canvas").GetComponent<DialogueQueue>();
    }

    public static void AddMessage(string message)
    {
        AddMessage(message, 2.5f + Mathf.Clamp((message.Length - 50) * 2.5f / 100f, 0, 2.5f));
    }
    public static void AddMessage(string message, float displayTime)
    {
        DialogueQueue dq = Get();
        if (!dq) Debug.LogError("No DialogQueue!");
        if (dq.messageQueue == null) dq.messageQueue = new Queue<TextMessage>();
        dq.messageQueue.Enqueue(new TextMessage(message, displayTime));
        if (!dq.showingMessages)
        {
            dq.showingMessages = true;
            dq.DisplayNextMessage();
            dq.textBoxRoot.gameObject.SetActive(true);
        }
    }

    private void DisplayNextMessage()
    {
        cooldownTimer = 0.5f;
        var m = messageQueue.Dequeue();
        text.text = m.Message;
        timer = m.Time;
        audioSource.PlayDelayed(0.3f);
    }
}
