using UnityEngine;

public class BullyScript : MonoBehaviour
{
	public Transform player;

	public GameControllerScript gc;

	public Renderer bullyRenderer;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float waitTime;

	public float activeTime;

	public float guilt;

	public bool active;

	public bool spoken;

	private AudioSource audioDevice;

	public AudioClip[] aud_Taunts = new AudioClip[2];

	public AudioClip[] aud_Thanks = new AudioClip[2];

	public AudioClip aud_Denied;

	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
		waitTime = Random.Range(60f, 120f);
	}

	private void Update()
	{
		if (waitTime > 0f)
		{
			waitTime -= Time.deltaTime;
		}
		else if (!active)
		{
			Activate();
		}
		if (active)
		{
			activeTime += Time.deltaTime;
			if ((activeTime >= 180f) & ((base.transform.position - player.position).magnitude >= 120f))
			{
				Reset();
			}
		}
		if (guilt > 0f)
		{
			guilt -= Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		Vector3 direction = player.position - base.transform.position;
		RaycastHit hitInfo;
		if (Physics.Raycast(base.transform.position + new Vector3(0f, 4f, 0f), direction, out hitInfo, float.PositiveInfinity, 3, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player") & bullyRenderer.isVisible & ((base.transform.position - player.position).magnitude <= 30f) & active)
		{
			if (!spoken)
			{
				int num = Mathf.RoundToInt(Random.Range(0f, 1f));
				audioDevice.PlayOneShot(aud_Taunts[num]);
				spoken = true;
			}
			//guilt = 10f;
		}
	}

	private void Activate()
	{
		wanderer.GetNewTargetHallway();
		base.transform.position = wanderTarget.position + new Vector3(0f, 5f, 0f);
		while ((base.transform.position - player.position).magnitude < 20f)
		{
			wanderer.GetNewTargetHallway();
			base.transform.position = wanderTarget.position + new Vector3(0f, 5f, 0f);
		}
		active = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Player")
		{
				int num = Mathf.RoundToInt(Random.Range(1, 10));
				gc.CollectItem(num);
				int num2 = Mathf.RoundToInt(Random.Range(0f, 1f));
				audioDevice.PlayOneShot(aud_Thanks[num2]);
				Reset();
		}
		if ((other.transform.name == "Principal of the Thing") & (guilt > 0f))
		{
			Reset();
		}
	}

	private void Reset()
	{
		base.transform.position = base.transform.position - new Vector3(0f, 20f, 0f);
		waitTime = Random.Range(60f, 120f);
		active = false;
		activeTime = 0f;
		spoken = false;
	}
}
