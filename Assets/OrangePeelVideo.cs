using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class OrangePeelVideo : MonoBehaviour
{
    public GameObject cam;
    public VideoClip clip;

    private List<Func<bool>> possibilities = new List<Func<bool>>() {
        () => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow),
        () => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow),
        () => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow),
        () => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow),
    };

    public float Timer;
    public float goodMultiplier = 1;

    private float currentTimer;
    private float correctTimer = 0;
    private float incorrectTimer = 0;
    public int currentCheck;
    public float playbackSpeed;
    public UnityEvent onEnd;
    public VideoRenderMode a;
    public RenderTexture texture;
    private VideoPlayer videoPlayer;

    void Start()
    {
        // Will attach a VideoPlayer to the main camera.
        GameObject camera = cam.gameObject;

        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();

        // Play on awake defaults to true. Set it to false to avoid the url set
        // below to auto-start playback since we're in Start().
        videoPlayer.playOnAwake = false;

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
         videoPlayer.renderMode = a;

        // This will cause our Scene to be visible through the video being played.
        videoPlayer.targetCameraAlpha = 0.5F;
        videoPlayer.targetTexture = texture;

        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        videoPlayer.clip = clip;

        // Skip the first 100 frames.
        videoPlayer.frame = 100;

        // Restart from beginning when done.
        videoPlayer.isLooping = true;

        // Each time we reach the end, we slow down the playback by a factor of 10.
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.EnableAudioTrack(0, false);

        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        onEnd.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer > Timer) {
            currentTimer = 0;
            currentCheck = UnityEngine.Random.Range(0, 4);
        }

        if(possibilities[currentCheck]()) {
            incorrectTimer = 0;
            correctTimer += Time.deltaTime;
        } else {
            correctTimer -= 0.3f*Time.deltaTime;
        }
        correctTimer = Mathf.Max(0, correctTimer);

        playbackSpeed = correctTimer * goodMultiplier;
        videoPlayer.playbackSpeed = playbackSpeed;
        
    }
}
