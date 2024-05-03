using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform Player;
    [SerializeField] private float Speed, maxLimit, minLimit;
    private float initialSize;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        initialSize = cam.orthographicSize;
    }

    void FixedUpdate(){
        FollowPlayer();
        //Zoom(15f, 2f, Input.GetAxis("Zoom") * 0.5f);
        StopInLimits();
    }

    public void FollowPlayer(){
        transform.position = Vector2.Lerp(transform.position, Player.position, Speed);
    }

    //provavelmente inútil
    public void Zoom(float maxSize, float minSize, float zoomSpeed){
        SetSize(cam.orthographicSize - zoomSpeed);

        if(cam.orthographicSize > maxSize){
             cam.orthographicSize = maxSize;
        }else if(cam.orthographicSize < minSize){
            cam.orthographicSize = minSize;
        }
    }

    void StopInLimits(){
        if((transform.position.x + cam.orthographicSize) > maxLimit){
            transform.position = new Vector3(maxLimit - cam.orthographicSize, transform.position.y, transform.position.z);
        }else if((transform.position.x - cam.orthographicSize) < minLimit){
            transform.position = new Vector3(minLimit + cam.orthographicSize, transform.position.y, transform.position.z);
        }
    }

    public void SetSize(float size){
        cam.orthographicSize = size;
    }

    public void AutoZoom(float VerticalRange, float ZoomRange, float RangeLeft, float RangeRight, float FinalSize, Transform Target){
        if((Player.position.y <= (Target.position.y + VerticalRange / 2f)) && (Player.position.y >= (Target.position.y - VerticalRange / 2f))){
            if((Player.position.x >= Target.position.x - RangeLeft) && (Player.position.x <= Target.position.x)){
                if(Player.position.x < Target.position.x - ZoomRange / 2){
                    SetSize(((FinalSize - initialSize) / (ZoomRange / 2f - RangeLeft)) * (Target.position.x - Player.position.x - RangeLeft) + initialSize);
                }else{
                    SetSize(FinalSize);
                }
            }else if((Player.position.x > Target.position.x + ZoomRange / 2) && (Player.position.x <= Target.position.x + RangeRight)){
                if(Player.position.x > Target.position.x + ZoomRange / 2){
                    SetSize(((FinalSize - initialSize) / (ZoomRange / 2f - RangeRight)) * (Player.position.x - Target.position.x - RangeRight) + initialSize);
                }else{
                    SetSize(FinalSize);
                }
            }
        }
    }
} 