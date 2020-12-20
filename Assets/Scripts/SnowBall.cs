using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    // Start is called before the first frame update
    int collisionTime = 0;
    private AudioSource _snowBallImpactSource;
    void Start()
    {
        _snowBallImpactSource = GetComponent<AudioSource>();
        if (_snowBallImpactSource == null)
        {
            _snowBallImpactSource = gameObject.AddComponent<AudioSource>();
            _snowBallImpactSource.clip = AudioSourceManager.Instance.SnowBallImpact;
            _snowBallImpactSource.playOnAwake = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != null)
        {
            _snowBallImpactSource.Play();
            if (collision.gameObject.tag.Equals("Player"))
            {
                Destroy(gameObject);
            }
            else
            {
                collisionTime++;
                if(collisionTime == 3)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void DestroyInterval(float interval)
    {
        Destroy(gameObject, interval);
    }
}
