using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public TowerProperties towerProperties;
    public Transform self;
    public ParticleSystem towerDestructionSFX;

    public GameObject towerBlocPrefab;
    public CapsuleCollider capsuleCollider;

    //private int currentHealth;

    private List<TowerBloc> blocs;

    public float fallDuration;

    private bool falling = false;
    private float timeLeftInAir;
    private float heightToRemove;

    public void Init(TowerProperties towerProperties)
    {
        for (int i = 0; i < self.childCount; i++)
        {
            Destroy(self.GetChild(i).gameObject);
        }

        this.towerProperties = towerProperties;
        self = transform;
        blocs = new List<TowerBloc>();
        float currentHeight=0;
        for (int i = 0; i < towerProperties.blocCount; i++)
        {
            blocs.Add(Instantiate(towerBlocPrefab, self).GetComponent<TowerBloc>());
            blocs[i].transform.localPosition = new Vector3(0,i*towerBlocPrefab.transform.localScale.y,0);
            blocs[i].Init(towerProperties.blocPattern[i%towerProperties.blocPattern.Count],this,i,currentHeight);
            currentHeight += blocs[i].mesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.y*blocs[i].self.localScale.y;
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
        capsuleCollider.enabled = false;
    }


    public void RemoveBloc(TowerBloc towerBloc)
    {
        blocs.Remove(towerBloc);
        falling = true;
        heightToRemove += towerBloc.mesh.GetComponent<MeshFilter>().mesh.bounds.size.y * towerBloc.self.localScale.y;
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
        MapManager.instance.RemoveTower(this);
        Destroy(gameObject);   
    }



    private void Update()
    {
        if (!falling) { return; }

        timeLeftInAir -= Time.deltaTime;
        if (timeLeftInAir > 0)
        {
            float percent =1-timeLeftInAir/fallDuration;

            for (int i = 0; i < blocs.Count; i++)
            {
                blocs[i].self.position = new Vector3(blocs[i].self.position.x, blocs[i].tower.self.position.y+ blocs[i].blocHeight - ( heightToRemove*percent), blocs[i].self.position.z);
            }
        }
        else
        {
            float currentHeight = 0;
            for (int i = 0; i < blocs.Count; i++)
            {
                blocs[i].blocHeight = currentHeight;
                blocs[i].self.position = new Vector3(blocs[i].self.position.x, blocs[i].tower.self.position.y +blocs[i].blocHeight, blocs[i].self.position.z);
                currentHeight += blocs[i].mesh.GetComponent<MeshFilter>().mesh.bounds.size.y * blocs[i].transform.localScale.y;
            }
            heightToRemove = 0;
            falling = false;
            ToggleBlocsInvincibility(false);
        }
    }


    public void ToggleBlocsInvincibility(bool on)
    {
        for (int i = 0; i < blocs.Count; i++)
        {
            blocs[i].invincible = on;
        }
        capsuleCollider.enabled = on;
    }


    public TowerBloc GetBlocAtIndex(int index)
    {
        return blocs[index];
    }


    public int GetBlocCount()
    {
        return blocs.Count;
    }

}
