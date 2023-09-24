using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MyHand : MonoBehaviour
{
    // private MeshCollider _meshCollider;
    //  private OVRMesh _ovrMesh;
    private SkinnedMeshRenderer _meshRenderer;

    private OVRHand _ovrHand;
    private OVRSkeleton _skeleton;
    private bool[] fingersPinched = new bool[5];
    [SerializeField] private bool isRightHand;
    private Vector3 indexFingerPosition;
    private Vector3 startPullPos;
    private References _references;
    private SoundManager _soundManager;
    private PlanetSpawnder _planetSpawnder;
    private const int boneFingerTips = 19;
    public static ButtonAction _buttonAction = ButtonAction.spawnPlanet;
    public static float planetSize;
    private int handIndex; //,fingerIndex;
    private Transform[] handColliders;

    private static MyHand leftHand, rightHand;

    void Awake()
    {
        // _meshCollider = GetComponent<MeshCollider>();
        // _ovrMesh = GetComponent<OVRMesh>();
        _meshRenderer = GetComponent<SkinnedMeshRenderer>();
        _ovrHand = GetComponent<OVRHand>();
        _skeleton = GetComponent<OVRSkeleton>();

        if (isRightHand)
        {
            handIndex = 0;
            rightHand = this;
        }
        else
        {
            handIndex = 1;
            leftHand = this;
        }
        
        //   fingerIndex = (isRightHand) ? 0 : 5;
    }

    private void Start()
    {
        _references = References.instance;
        _soundManager = SoundManager.instance;
        _planetSpawnder = PlanetSpawnder.instance;
        handColliders = new Transform[32];
        for (int i = 0; i < handColliders.Length; i++)
        {
            handColliders[i] = Instantiate(_references.fingerColliderPrefab, transform).transform;
        }
        
        
    }

    void FixedUpdate()
    {
        UpdateHandColliders();
        indexFingerPosition = GetFingertipPosition(Finger.Index);
        for (int i = 0; i < fingersPinched.Length; i++)
        {
            SetPinchedFinger(i, _ovrHand.GetFingerIsPinching((OVRHand.HandFinger)i));
        }


        if (_buttonAction == ButtonAction.gravityHands)
        {
            var ball = _references.gravityHandBalls[handIndex];
            ball.SetActive(true);
            ball.transform.position = handColliders[29].position;
            _meshRenderer.material = _references.gravityHandMaterial;
        }
        else
        {
            if (_meshRenderer.material != _references.defaultHandMaterial)
            {
                var ball = _references.gravityHandBalls[handIndex];
                ball.SetActive(false);
                _meshRenderer.material = _references.defaultHandMaterial;
            }
        }

        WhilePinching();
    }



    
    
    
    
    
    private bool createdArrows;
    private Transform indexArrow, thumbArrow,pinkyArrow;
    private float arrowHiddenTime = 60f;
    private float arrowHiddenTimer;

    
    
    public static void CreateArrowsForBothHands()
    {
        leftHand.CreateArrows();
        rightHand.CreateArrows();
    }


    public void CreateArrows()
    {
        indexArrow = Instantiate(_references.arrowPrefab).transform;
        thumbArrow = Instantiate(_references.arrowPrefab).transform;
        pinkyArrow = Instantiate(_references.arrowPrefab).transform;
        createdArrows = true;
    }
    
    
    private void HideArrows()
    {
        indexArrow.gameObject.SetActive(false);
        thumbArrow.gameObject.SetActive(false);
        pinkyArrow.gameObject.SetActive(false);
        arrowHiddenTimer = arrowHiddenTime;
    }
    
    
    private void Update()
    {
        if (createdArrows)
        {
            bool arrowIsHidden = (arrowHiddenTimer > 0);

            if (arrowIsHidden)
            {
                arrowHiddenTimer -= Time.deltaTime;
            }
            else
            {
                indexArrow.position = indexFingerPosition;
                pinkyArrow.position = GetFingertipPosition(Finger.Pinky);
                thumbArrow.position = GetFingertipPosition(Finger.Thumb);
           
                indexArrow.LookAt(thumbArrow);
                pinkyArrow.LookAt(thumbArrow);
                thumbArrow.LookAt(indexArrow);
          
            }

        }
        
        
       
    }

    private void UpdateHandColliders()
    {
        for (int i = 0; i < 24; i++)
        {
            handColliders[i].position = GetBonePos(i);
        }

        var basePos = GetBonePos(0);
        handColliders[24].position = basePos + (GetBonePos(6) - basePos) / 2;
        handColliders[25].position = basePos + (GetBonePos(9) - basePos) / 2;
        handColliders[26].position = basePos + (GetBonePos(12) - basePos) / 2;
        handColliders[27].position = basePos + (GetBonePos(16) - basePos) / 2;


        handColliders[28].position = basePos + (GetBonePos(6) - basePos) / 1.3f;
        handColliders[29].position = basePos + (GetBonePos(9) - basePos) / 1.3f;
        handColliders[30].position = basePos + (GetBonePos(12) - basePos) / 1.3f;
        handColliders[31].position = basePos + (GetBonePos(16) - basePos) / 1.3f;


        _references.gravityBall.position = handColliders[30].position;
    }

    private Vector3 GetBonePos(int boneIndex)
    {
        if (boneIndex >= _skeleton.Bones.Count)
        {
            return Vector3.zero;
        }
        else
        {
            return _skeleton.Bones[boneIndex].Transform.position;
        }
    }

    private Vector3 GetFingertipPosition(Finger finger)
    {
        return GetBonePos(boneFingerTips + (int)finger);
    }

    private MyController currentController;

    public async Task SetCurrentController(MyController controller)
    {
        if (currentController == null || controller == null)
            currentController = controller;
    }

    private void DisposeLineDisplay()
    {
        MyLine.DisposeLine(handIndex);
    }

    private void StartDisplaingControll()
    {
        _references.text.text = "";
        _soundManager.PlaySound(Sound.pull);
        var controllerColor = currentController.GetMaterialColor();
        MyLine.MakeLine(handIndex, controllerColor, true, 0.03f, 0.03f);
    }

    private int count;

    private Planet grabbedPlanet;
    /////////////////////////////////////////////////////      ///////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////      ///////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////      ///////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////      ///////////////////////////////////////////////////////////////////

    private void OnPinchStart(Finger finger)
    {
        if (currentController != null && finger == Finger.Index)
        {
            StartDisplaingControll();
            if (currentController._controllerAction == ControllerAction.planetScale)
            {
                _references.sizeDisplay.SetActive(true);
            }

            return;
        }

        if (_buttonAction == ButtonAction.nothing) return;


        if (finger == Finger.Index)
        {
            HideArrows();
            
            if (_buttonAction == ButtonAction.pinchMode)
            {
                var hits = Physics.SphereCastAll(indexFingerPosition, 0.5f, Vector3.zero, 10, LayerMask.GetMask("Planet"));
                grabbedPlanet = hits[0].transform.GetComponent<Planet>();
                grabbedPlanet.transform.parent = transform;
                grabbedPlanet.MakeStationary();
            }
            else if (_buttonAction == ButtonAction.countingMode)
            {
                if (count == 0)
                    _references.countBall.gameObject.SetActive(true);

                count++;
                _references.countBall.position = GetBonePos(count);
                _references.text2.text = count.ToString();
            }
            else if (_buttonAction == ButtonAction.spawnPlanet)
            {
                _soundManager.PlaySound(Sound.pull);
                MyLine.MakeLine(handIndex, Color.yellow, true, 0.05f, 0.02f);
                startPullPos = indexFingerPosition;
            }else if  (_buttonAction == ButtonAction.restart)
            {
                _buttonAction = ButtonAction.nothing;
                 Loader.ReloadLevel();
                
            }
        }
        else if (finger == Finger.Pinky)
        {
            if (_buttonAction == ButtonAction.countingMode)
            {
                count = 0;
                _references.countBall.gameObject.SetActive(false);
                _references.text2.text = "";
            }
            else if (_buttonAction == ButtonAction.draw)
            {
                // _references.drawBalls[handIndex].GetComponent<TrailRenderer>().Clear();
                // ClearDrawing();
                RandomizeDrawingColor();
            }
            else if (_buttonAction == ButtonAction.spawnPlanet)
            {
                
                Instantiate(_references.earthPrefab, GetFingertipPosition(Finger.Pinky), Quaternion.identity);
                // _planetSpawnder.DisableAll();
                // MyBoundaryManager.instance.DisposeAllIstantiatedGameObjects();
            }

            _soundManager.PlaySound(Sound.swoosh);
        }
        else if (finger == Finger.Middle)
        {
            
            
            // if (_buttonAction == ButtonAction.draw)
            // {
            //   RandomizeDrawingColor();
            // }
            
            
            
            if (_buttonAction == ButtonAction.countingMode)
            {
                if (count > 0)
                    count--;

                _references.countBall.position = GetBonePos(count);
                _references.text2.text = count.ToString();
            }
            else
            {
                //  MyBoundaryManager.instance.SpawnOuterBoundary();
            }
        }
        else if (finger == Finger.Ring)
        {
            //   MyBoundaryManager.instance.SpawnPlayAreaBoundary();
        }
    }

    private void WhilePinching()
    {
        if (currentController != null && IsPinched(Finger.Index))
        {
            var controllerAction = currentController._controllerAction;
            var controllerPosition = currentController.transform.position;
            float value = (indexFingerPosition - controllerPosition).magnitude * 10;
            MyLine.UpdateLinePosition(handIndex, controllerPosition, indexFingerPosition);
            //     _references.balls[handIndex].position = indexFingerPosition;
            if (controllerAction == ControllerAction.musicVolume)
            {
                value /= 10;
                MusicManager._instance.SetVolume(value); //Divide this by 10 to get from 0 to 1
            }

            if (controllerAction == ControllerAction.gravityForce)
            {
                value /= 100;
                _references.gravitationalForce = value;
            }
            else if (controllerAction == ControllerAction.slapForce)
            {
                _references.slapForce = value;
            }
            else if (controllerAction == ControllerAction.planetScale)
            {
                value /= 10;
                planetSize = value;
            }

            value = ((int)(value * 100)) / 100f;
            _references.text.text = value.ToString();
            return;
        }


        if (_buttonAction == ButtonAction.nothing) return;

        if (IsPinched(Finger.Index))
        {
            if (_buttonAction == ButtonAction.draw)
            {
                // _references.drawBalls[handIndex].position = indexFingerPosition;
                Draw();
            }
            else if (_buttonAction == ButtonAction.spawnPlanet)
            {
                MyLine.UpdateLinePosition(handIndex, startPullPos, indexFingerPosition);
            }
        }
    }


    private void OnPinchEnd(Finger finger)
    {
        if (currentController != null && finger == Finger.Index)
        {
            if (currentController._controllerAction == ControllerAction.planetScale)
            {
                _references.sizeDisplay.SetActive(false);
            }

            currentController = null;

            return;
        }

        if (_buttonAction == ButtonAction.nothing) return;

        if (finger == Finger.Index)
        {
            if (_buttonAction == ButtonAction.pinchMode)
            {
                grabbedPlanet.transform.parent = null;
                grabbedPlanet.MakeMovable();
            }
            else if (_buttonAction == ButtonAction.spawnPlanet)
            {
                _planetSpawnder.SpawnPlanet(startPullPos, startPullPos - indexFingerPosition, planetSize);
            }
            else if (_buttonAction == ButtonAction.spawnDoesNotGetPulled)
            {
                _planetSpawnder.SpawnDoesNotGetPulled(startPullPos, startPullPos - indexFingerPosition, planetSize);
            }
            else if (_buttonAction == ButtonAction.spawnStatonaryPlanet)
            {
                _planetSpawnder.SpawnStatonary(indexFingerPosition, planetSize);
            }
            else if (_buttonAction == ButtonAction.spawnDoesNotPull)
            {
                _planetSpawnder.SpawnDoesNotPull(indexFingerPosition, startPullPos - indexFingerPosition, planetSize);
            }
            else if (_buttonAction == ButtonAction.draw)
            {
                // _references.drawBalls[handIndex].position = indexFingerPosition;
                StopDrawing();
            }

            _soundManager.PlaySound(Sound.click);
            DisposeLineDisplay();
        }
    }


    private Transform drawParent;
    private LineRenderer currentLine;
    private Vector3 lastDrawPos;
    private float drawDistance = 0.005f;
    private static Color drawingColor = Color.yellow;

    private void Draw()
    {
        if (drawParent == null)
        {
            drawParent = new GameObject("DrawParent").transform;
        }

        if (currentLine == null)
        {
            // currentLine = new GameObject("Line").transform;

            currentLine = MyLine.InstantiateLine(drawingColor,false,0.01f);
            currentLine.positionCount = 0;
        }
        
        Vector3 pointToDraw = indexFingerPosition;
        
        
        
        if(Vector3.Distance(pointToDraw, lastDrawPos) > drawDistance)
        {

            lastDrawPos = pointToDraw;
            currentLine.positionCount++;
            currentLine.SetPosition(currentLine.positionCount - 1, pointToDraw);
        }

        // _references.drawBalls[handIndex].position = indexFingerPosition;
    }
    
    
    private void RandomizeDrawingColor()
    {
        drawingColor = Random.ColorHSV();
        _references.drawSpriteRenderer.color = drawingColor;
    }

    private void StopDrawing()
    {
        currentLine = null;
    }

    // private void ClearDrawing()
    // {
    //     // _references.drawBalls[handIndex].GetComponent<TrailRenderer>().Clear();
    //
    //     if (drawParent != null)
    //     {
    //         foreach (Transform child in drawParent)
    //         {
    //             child.gameObject.SetActive(false);
    //         }
    //     }
    // }

    /////////////////////////////////////////////////////      ///////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////      ///////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////      ///////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////      ///////////////////////////////////////////////////////////////////
    private void SetPinchedFinger(int index, bool newBool)
    {
        if (fingersPinched[index] == newBool) return;

        fingersPinched[index] = newBool;
        Finger pinchedFinger = (Finger)index;
        if (newBool)
        {
            OnPinchStart(pinchedFinger);
        }
        else
        {
            OnPinchEnd(pinchedFinger);
        }
    }


    private bool IsPinched(Finger finger)
    {
        return _ovrHand.GetFingerIsPinching((OVRHand.HandFinger)finger);
    }

    private enum Finger
    {
        Thumb = 0,
        Index = 1,
        Middle = 2,
        Ring = 3,
        Pinky = 4
    }
}