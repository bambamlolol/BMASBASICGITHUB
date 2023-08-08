using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public Text debugText;
    private float fps;
    private float lastFrameTime;
    private int totalFrames;
    private float averageFrameTime;

    private void Start()
    {
        // Set up the debug text
        debugText.text = "Debug Info:";
        lastFrameTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        // Calculate FPS
        fps = 1.0f / Time.deltaTime;
        
        // Calculate average frame time
        totalFrames++;
        float currentFrameTime = Time.realtimeSinceStartup;
        float frameTime = currentFrameTime - lastFrameTime;
        lastFrameTime = currentFrameTime;
        averageFrameTime += (frameTime - averageFrameTime) / totalFrames;
        
        // Update the debug text
        debugText.text = string.Format("Debug Info:\nFPS: {0}\nAverage Frame Time: {1}ms\nTime Scale: {2}\nPosition: {3}\nRotation: {4}\n",
            fps.ToString("0.00"), (averageFrameTime * 1000f).ToString("0.00"), Time.timeScale, transform.position, transform.rotation.eulerAngles);
    }
}
