using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeTargetBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Agents")) //ball touched purple goal
        {
            col.gameObject.GetComponent<CapsuleAgent>().addReward(-1);
            
            
            Vector3 position = new Vector3(Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min,
                0.5f,
                Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min);

            this.transform.position = position;
        }
    }
}
