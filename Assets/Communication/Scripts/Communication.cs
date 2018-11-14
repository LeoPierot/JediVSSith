using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.Events;
using System.Collections.Generic;

public class Communication : MonoBehaviour {

	[System.Serializable]
	public struct voiceEvent
	{
		public string command;
		public UnityEvent actionEvent;
		public char key;
	}

	public KeywordRecognizer speachApi = null;
	private List<String> keywords;

	public voiceEvent[] voiceEvents;
	
	// Use this for initialization
	void Start () {
		keywords = new List<String>();
		foreach (voiceEvent element in voiceEvents)
		{
			keywords.Add(element.command);
		}

		if(!this.InitializeCommunication()){
			return;
		}
		Debug.Log("Communication initialized");
	}

	void Update(){

		foreach (voiceEvent element in voiceEvents)
		{
			if (Input.GetKeyDown(element.key.ToString())) {
				element.actionEvent.Invoke();
			}
		}
	}

	void OnDestroy(){
		this.speachApi.OnPhraseRecognized -= OnVoiceRecognition;
		this.speachApi.Stop();
	}


	bool InitializeCommunication(){
		if(!PhraseRecognitionSystem.isSupported){
			return false;
		}
		if(this.voiceEvents == null){
			return false;
		}

		foreach (var item in keywords)
		{
			print(item);
		}

		this.speachApi = new KeywordRecognizer(this.keywords.ToArray());
		this.speachApi.OnPhraseRecognized += OnVoiceRecognition;
		this.speachApi.Start();
		return true;
	}

	void OnVoiceRecognition(PhraseRecognizedEventArgs args){

		for (int i = 0; i < voiceEvents.Length; i++)
		{
			if(voiceEvents[i].command.ToLower() == args.text.ToLower())
			{
				voiceEvents[i].actionEvent.Invoke();
			}	
		}

		// StringBuilder stringBuilder = new StringBuilder();
		// stringBuilder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        // stringBuilder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        // stringBuilder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        // Debug.Log(stringBuilder.ToString());
	}
}
