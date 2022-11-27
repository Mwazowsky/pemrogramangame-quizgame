using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   private GameObject audioManagerObj;

   private AudioManager audioScript;
   
   void Start()
   {
         audioManagerObj =  GameObject.Find("/AudioManager");
         
         audioScript = (AudioManager)audioManagerObj.GetComponent(typeof(AudioManager));
   }

   void Update()
   {
      if (audioScript == null)
      {
         audioManagerObj =  GameObject.Find("/AudioManager");
         audioScript = (AudioManager)audioManagerObj.GetComponent(typeof(AudioManager));
      }
   }

   public void PlayClickFX()
   {
      audioScript.PlaySound("ClickSound");
   }
   public void LoadNextScene()
   {
      SceneManager.LoadScene(1);
   }
   public void LoadPreviousScene()
   {
      SceneManager.LoadScene(0);
   }
   
   public void LoadHewanScene()
   {
      SceneManager.LoadScene(2);
   }
   public void LoadTumbuhanScene()
   {
      SceneManager.LoadScene(3);
   }
   public void LoadTransportasiScene()
   {
      SceneManager.LoadScene(4);
   }
   
   public void LoadKnowledgeHewanScene()
   {
      SceneManager.LoadScene(5);
   }
   public void LoadKnowledgeTumbuhanScene()
   {
      SceneManager.LoadScene(6);
   }
   public void LoadKnowledgeTransportasiScene()
   {
      SceneManager.LoadScene(7);
   }
}
