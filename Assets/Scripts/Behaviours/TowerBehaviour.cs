using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    [HideInInspector] public TowerProperties towerProperties;
    public Transform self;
    public ParticleSystem towerDestructionSFX;

    public GameObject towerBlocPrefab;

    //private int currentHealth;

    private List<TowerBloc> blocs;

    public float fallDuration;

    private bool falling = false;
    private float timeLeftInAir;

    public void Init(TowerProperties towerProperties)
    {
        this.towerProperties = towerProperties;
        self = transform;
        blocs = new List<TowerBloc>();
        for (int i = 0; i < towerProperties.blocCount; i++)
        {
            blocs.Add(Instantiate(towerBlocPrefab, self).GetComponent<TowerBloc>());
            blocs[i].transform.position = new Vector3(0,i*towerBlocPrefab.transform.localScale.y,0);
            blocs[i].Init(towerProperties.blocPattern[i%towerProperties.blocPattern.Count],this,i);
        }
        switch (towerProperties.towerStructure)
        {
            case TowerStructure.Straight:
                //do nothing LUL
                break;
            case TowerStructure.Spiral:
                for (int i = 0; i < blocs.Count; i++)
                {
                    blocs[i].transform.eulerAngles = new Vector3(0, i*15, 0);
                }
                break;
            case TowerStructure.Random:
                for (int i = 0; i < blocs.Count; i++)
                {
                    blocs[i].transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
                }
                break;
        }
    }


    public void RemoveBloc(TowerBloc towerBloc)
    {
        blocs.Remove(towerBloc);
        falling = true;
        timeLeftInAir = fallDuration;
        if (blocs.Count == 0)
        {
            Debug.Log("Tower destroyed");
            StartCoroutine(Die());
        }
    }


    private IEnumerator Die()
    {
        towerDestructionSFX.Play();
        
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);   
    }



    private void Update()
    {
        if (!falling) { return; }

        timeLeftInAir -= Time.deltaTime;
        if (timeLeftInAir > 0)
        {
            float percent = 1-Mathf.InverseLerp(0, fallDuration, timeLeftInAir);
            for (int i = 0; i < blocs.Count; i++)
            {
                blocs[i].self.position = new Vector3(blocs[i].self.position.x, (blocs[i].blocIndex - percent)* blocs[i].self.localScale.y, blocs[i].self.position.z);
            }
        }
        else
        {
            for (int i = 0; i < blocs.Count; i++)
            {
                blocs[i].self.position = new Vector3(blocs[i].self.position.x, --blocs[i].blocIndex*blocs[i].self.localScale.y, blocs[i].self.position.z);
            }
            falling = false;
            ToggleBlocColliders(true);
        }
    }


    public void ToggleBlocColliders(bool on)
    {
        for (int i = 0; i < blocs.Count; i++)
        {
            blocs[i].meshCollider.enabled = on;
        }
    }
}
