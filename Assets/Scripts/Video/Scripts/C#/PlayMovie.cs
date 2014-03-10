
using UnityEngine;
using System.Collections;
using ETraining.UI.VideoControls;
using ETraining.UI.StepControls;

[RequireComponent (typeof (GUITexture))]
[RequireComponent (typeof (AudioSource))]

public class PlayMovie : MonoBehaviour {
	#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    public MovieTexture movie;
  	#endif	

	
	public float _movieTimer;
	public bool isPlay = false;
	public bool isPause = false;
	public bool isStop = false;
	public GUISkin mySkin;
	 
	public AudioSource soure;

	private int videoWidth = 800;
	private int videoHeight = 600;
	private int videoLeftRightBorder = 10;
	private int videoUpBorder = 30;
	
	private int topLeftX;
	private int topLeftY;
	private int playerWidth;
	private int playerHeight;

	public string VideoName {
		get {
			return this.videoName;
		}
		set {
			videoName = value;
		}
	}

	#if UNITY_STANDALONE_WIN || UNITY_EDITOR
	private StopButton stopBtn;
	private VolumeSlider volumeSlider;
	#endif
	private string videoName;
	private WWW www ;
	IEnumerator WaitInSeconds(float waitTime) {
        yield return new WaitForSeconds(waitTime);        
    }
	
	IEnumerator WaitToLoadMovie() 
	{        
        yield return StartCoroutine(WaitInSeconds(1.0F));   			
		
    }
	
	
	void Start()
	{

		videoWidth = Mathf.CeilToInt(((float)Screen.width / (float)1920) * (float)videoWidth);
		videoHeight = Mathf.CeilToInt(((float)Screen.height / (float)1080) * (float)videoHeight);
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		volumeSlider = GameObject.Find("VolumeSlider").GetComponent(typeof(VolumeSlider)) as VolumeSlider; 
		stopBtn = GameObject.Find("StopButton").GetComponent(typeof(StopButton)) as StopButton; 
		#endif
		//StartCoroutine(WaitToLoadMovie()) ;
	}
	public void setVideo(string videoName)
	{	
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		www = new WWW("file://" + Application.dataPath + "/StreamingAssets/Videos/" + videoName);	
		Debug.Log("file://" + Application.dataPath + "/StreamingAssets/Videos/" + videoName);
		movie = www.movie;			
		soure.clip = movie.audioClip;		
		this.videoName = videoName;
		#endif
	}

	public void releaseVideo()
	{	
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR

		movie = null;			
		soure.clip = null;		
		//this.videoName = videoName;
		#endif
	}
	
	private int playCount = 0;
	private int pauseCount = 0;
	private int stopCount = 0;	


	void OnGUI()
	{
		//if(!canRunVideo) return;

		
		GUI.skin = mySkin;
		ShowButtonInfo scriptShowInfo = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
		
		//GUI.DrawTexture(new Rect(Screen.width/2 - 400 - scriptShowInfo.getZeroToBoxInfoWidth()/2, Screen.height/2 - 300, 800, 600), movie);
		ShowButtonInfo script = GameObject.Find("ShowInfoButton").GetComponent(typeof(ShowButtonInfo)) as ShowButtonInfo;
		
		topLeftX = Screen.width/2 - videoWidth/2 - videoLeftRightBorder - script.getZeroToBoxInfoWidth()/2;
		topLeftY = Screen.height/2 - videoHeight/2 - videoUpBorder;
		playerWidth = 2 * videoLeftRightBorder + videoWidth;
		playerHeight = videoHeight + videoUpBorder;
		//script.x + script.XSize +  

		
		GUI.Box (new Rect(topLeftX, topLeftY, playerWidth, playerHeight), "");
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		if(movie != null && movie.isReadyToPlay)
		{
			GUI.DrawTexture(new Rect(topLeftX + videoLeftRightBorder, topLeftY + videoUpBorder, videoWidth, videoHeight), movie);
			if( movie.isPlaying)
			{		
			
				//soure.PlayClipAtPoint(movie.audioClip, new Vector3(0, 0, 0));
				FrameSlider fsScript = GameObject.Find("FrameSlider").GetComponent(typeof(FrameSlider)) as FrameSlider;				
				if (fsScript.checkEndPosition(scriptShowInfo)) fsScript.increasePosition(0);
				else 
				{
					//if(videoName == "Step1.ogv")
						fsScript.increasePosition(2);	

//					else if(videoName == "52-Step2.1.ogv")
//					{
//						increaseCOunt++;
//						if(increaseCOunt == 10000) increaseCOunt = 0;
//						if(increaseCOunt % 13 == 0)
//							fsScript.increasePosition(1);
//					}
//					else if(videoName == "51-step2.2.ogv")
//					{
//						increaseCOunt++;
//						if(increaseCOunt == 10000) increaseCOunt = 0;
//						if(increaseCOunt % 4 == 0)
//							fsScript.increasePosition(1);
//					}
//					else if(videoName == "50-step3.4.ogv")
//					{
//						increaseCOunt++;
//						if(increaseCOunt == 10000) increaseCOunt = 0;
//						if(increaseCOunt % 6 == 0)
//							fsScript.increasePosition(1);
//					}
//					else
//					{}
				}
			}
			else 
			{
				FrameSlider fsScript = GameObject.Find("FrameSlider").GetComponent(typeof(FrameSlider)) as FrameSlider;				
				if (isPause) fsScript.increasePosition(0);	
				if (fsScript.checkEndPosition(scriptShowInfo)) 
				{					
					setStop();
					PlayButton playBtn = GameObject.Find("PlayButton").GetComponent(typeof(PlayButton)) as PlayButton;
					playBtn.Used = false;
					
				}
				
			}								
		}
		
		if(stopBtn.used)
		{
			setStop();
			stopBtn.used = false;
			PlayButton playBtn = GameObject.Find("PlayButton").GetComponent(typeof(PlayButton)) as PlayButton;
			playBtn.Used = false;
		}
			
//		if
//		if(isPlay)
//		{
//			movie.Play ();				
//		}
//		if(isStop)	
//		{
//			movie.Stop();	
//			//if (!movie.isPlaying) setStop();
//		}
//		if(isPause) 
//		{
//			movie.Pause();							
//		}
		if (isStop) 
		{
			FrameSlider fsScript = GameObject.Find("FrameSlider").GetComponent(typeof(FrameSlider)) as FrameSlider;	
			fsScript.setBegin();
			movie.Stop();
			// reset first black background
			movie = www.movie;
			soure.clip = movie.audioClip;
			
		}
		if (movie.isPlaying && isPause) movie.Pause();
		if (isPlay) {
			movie.Play();	
		
		}
//		GUI.Box (new Rect(100, 50, 200, 50),movie.duration + "");
		if(soure.volume < 0.0f) soure.volume = 0.0f;
		else if(soure.volume > 1.0f) soure.volume = 1.0f;
		else
		{
			soure.volume += 0.05f *(float) volumeSlider.VolumeUp;
		}
			
		if(movie.isPlaying && playCount == 0) 
		{
			
			soure.Play();
			playCount++;
			if(pauseCount != 0) pauseCount = 0;
			if(stopCount != 0)stopCount = 0;
		}
		if(!movie.isPlaying && isPause && pauseCount == 0) 
		{
			soure.Pause();
			if(playCount != 0) playCount = 0;
			pauseCount++;
			if(stopCount != 0) stopCount = 0 ;
		}
		if(!movie.isPlaying && isStop && stopCount == 0)
		{
			//	soure.clip = movie.audioClip;
			soure.Stop();		
			
			soure.clip = movie.audioClip;
			if(playCount != 0) playCount = 0;
			if(pauseCount != 0) pauseCount = 0;
			stopCount++;
		}
			
		
		#endif
		
		//hSliderValue = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), hSliderValue, 0.0f, 10.0f);
		
	}
	
	void Update()
	{

	}


	
	public void setPlay()
	{
		isPlay = true;
		isPause = false;
		isStop = false;
	}
	
	public void setPause()
	{
		isPause = true;
		isPlay = false;
		isStop = false;
	}
	
	public void setStop()
	{
		isStop = true;
		isPause = false;
		isPlay = false;
	}

	public int VideoHeight {
		get {
			return this.videoHeight;
		}
		set {
			videoHeight = value;
		}
	}

	public int VideoLeftRightBorder {
		get {
			return this.videoLeftRightBorder;
		}
		set {
			videoLeftRightBorder = value;
		}
	}

	public int VideoUpBorder {
		get {
			return this.videoUpBorder;
		}
		set {
			videoUpBorder = value;
		}
	}

	public int VideoWidth {
		get {
			return this.videoWidth;
		}
		set {
			videoWidth = value;
		}
	}		
}

