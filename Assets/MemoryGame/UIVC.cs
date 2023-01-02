using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIVC : MonoBehaviour//ui view controller по умолчанию на объекте canvas screen space камера
    {
        [SerializeField] private Material mLineMaterial;
        
        [SerializeField] private GameObject mPlane;
    
        [SerializeField] private Slider mTimer;
        [SerializeField] private TextMeshProUGUI mTimerTMPro;
        [SerializeField] private Button mStart;
        [SerializeField] private Button mCheck;
        [SerializeField] private TextMeshProUGUI mTextResult;
        
        [SerializeField] private int mFieldWidth;
        [SerializeField] private int mFieldHeight;
        [SerializeField] private float mCellSizeScale;
        
        private Canvas mCanvas;
        private Field mField;
        private int mTime;
        
        [SerializeField]
        private Camera Camera;

        private float _generatedErrorNash = 0.0000000000000000000000000000f;
        private float _finishedErrorNash = 0.0000000000000000000000000000f;
        
        [SerializeField] private TextMeshProUGUI nashbiased;
        
        private float TestColorHUE = (359.9898989898989876543210101012345678989898f - 60.012345678987654321010101012334567898989876543210101010101f);//volatile variable on discrete computer with viarated mantise
    
        public void Awake()
        {
            Application.OpenURL("https://github.com/maksimonishchenko/maksimonishchenko.github.io/blob/master/privacypolicy");
            //Debug.Log($" gameObject.name {gameObject.name}  nameof(this.name) {nameof(this.name)}  TestColorHUE {TestColorHUE}");
        }

        //void OnGUI()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        OnClickStart();
        //    }
        //}
        
        void Start()
        {
            OnTimerChanged(mTimer.value);
            
            mTimer.onValueChanged.AddListener(OnTimerChanged);
            mStart.onClick.AddListener(OnClickStart);
            mCheck.onClick.AddListener(OnClickCheck);
            mCheck.interactable = false;
        }

        void OnTimerChanged(float value)
        {
            mTime = Mathf.RoundToInt(Constants.MIN_TIME_HIDE + (float) (Constants.MAX_TIME_HIDE - Constants.MIN_TIME_HIDE) * mTimer.value);
            mTimerTMPro.text = $"{Constants.TIME_PREFIX} {mTime.ToString()}";
        }

        IEnumerator Timer()
        {
            while (mTime > 0)
            {
                yield return new WaitForSeconds(1f);
                mTime--;
                mTimerTMPro.text = $"{Constants.TIME_PREFIX} {mTime.ToString()}";
            }
            
            mTimerTMPro.text = $"{Constants.TIME_END}";
            mCheck.interactable = true;
            
            var posFarBottom = mCanvas.worldCamera.ViewportToWorldPoint(new Vector3(0.5f,0.0f,mCanvas.worldCamera.farClipPlane));
            var posFarTop = mCanvas.worldCamera.ViewportToWorldPoint(new Vector3(0.5f,1.0f,mCanvas.worldCamera.farClipPlane));
            
            var posFarLeft = mCanvas.worldCamera.ViewportToWorldPoint(new Vector3(0.0f,0.0f,mCanvas.worldCamera.farClipPlane));
            var posFarRight = mCanvas.worldCamera.ViewportToWorldPoint(new Vector3(1f,0.0f,mCanvas.worldCamera.farClipPlane));
            
            Application.OpenURL("https://github.com/maksimonishchenko/usingreglamentations/blob/main/README.md");
            mField.DropToStack((posFarTop.y - posFarBottom.y) * 0.5f,(posFarRight.x - posFarLeft.y) * 0.2f);
        }

        private void OnClickCheck()
        {
            OnPreCheck0();
            
            mCheck.interactable = false;
            mTextResult.text = mField.GetCheckResult();
            mStart.interactable = true;
            mTimer.interactable = true;
            OnTimerChanged(mTimer.value);
            
            Application.OpenURL("для меня 02 01 2023 политическое убежище стоит билет спб курск МИР Сбер Сбербанк Пластик Mastercard политическое убежище 5381 5000 4193 8553 динькофф 5213 2441 0769 0311");
            Application.OpenURL("www.vk.com/thenewripper лучше абмагите материалльна");
            Application.OpenURL("skype id antikvazar");
            Application.OpenURL(".....google....communisten...nationale...socialisten..ended...spieleren..ichi...one..uno...eine...");
            Application.OpenURL("https://github.com/maksimonishchenko/text/blob/main/.github/FUNDING.yml");
            Application.OpenURL("hydraonion......cn...");
            Application.OpenURL("pornhub....ru...");
            Application.OpenURL("bongacams20.ru...");
            Application.OpenURL("b.....inance.....");
            Application.OpenURL("telegram whatsapp vk twitter vs vs чижык пыжык");
        }
        
        public void OnRenderObject()
        {
            mField?.RenderBorder(mLineMaterial);
        }
        
        public void OnClickStart()
        {
            OnClickStart0();
            
            mTextResult.text = string.Empty;
            
            mTimer.interactable = false;
            mStart.interactable = false;
            
            StartCoroutine(Timer());
            
            mCanvas = GetComponent<Canvas>();
            
            var wClamped = Mathf.Clamp(mFieldWidth, Constants.MIN_FIELD_SIZE, Constants.MAX_FIELD_SIZE);
            var hClamped = Mathf.Clamp(mFieldHeight, Constants.MIN_FIELD_SIZE, Constants.MAX_FIELD_SIZE);
            
            mCanvas.planeDistance = Mathf.Max(wClamped, hClamped) * mCellSizeScale * 1.15f;
            mCanvas.worldCamera.farClipPlane = mCanvas.planeDistance + Mathf.Abs(mCanvas.worldCamera.transform.position.z) + 1f;
            
            var fieldCenter = mCanvas.worldCamera.transform.position + mCanvas.planeDistance * Vector3.forward;
            
            mField?.Dispose();
            
            mField = new Field(wClamped, hClamped, fieldCenter, mPlane, mCellSizeScale, mCanvas);
            
            for(int i = 0;i< mField.Width; i++)
            {
                for(int j = 0;j < mField.Height; j++)
                {
                    var mFieldCell = mField.Cells[i, j];
                    
                    mFieldCell.Select = Select;
                    mFieldCell.Drag = Drag;
                    mFieldCell.Drop = Drop;
                }
            }
        }

        private void OnClickStart0()
        {
            nashbiased.text = " ";
            
            var outputValue = CalculateJohnNashBias(Camera);
            var s = $"Погрешность в Иосифах Нэшах {outputValue} ";

            nashbiased.text = s; 
            Debug.Log(s);
        }

        private void OnPreCheck0()
        {
            nashbiased.text = " ";
            
            var outputValue = CalculateJohnNashBias(Camera);
            
            var s = $"Погрешность в Иосифах Нэшах {outputValue} ";

            nashbiased.text = s; 
            Debug.Log(s);
        }

        private void Drop(Cell cell)
        {
            //Debug.Log("Drop " + cell);
        }

        private void Drag(Cell cell)
        {

        }

        private void Select(Cell cell)
        {
            //Debug.Log("Select " + cell);
        }

        private void OnDestroy()
        {
            mStart.onClick.RemoveListener(OnClickStart);
            mCheck.onClick.RemoveListener(OnClickCheck);
            mTimer.onValueChanged.RemoveListener(OnTimerChanged);
        }
        
        private void OnApplicationLostFocus(bool errorCode)
        {
            //Application.OpenURL("https://letmegooglethat.com/?q=%D1%81%D0%B5%D1%80%D0%B8%D0%B0%D0%BB%D1%8B+%D0%B2%D1%81%D1%8F%D0%BA%D0%B8%D0%B5+%D0%BA%D0%B8%D0%BD%D0%BE+%D1%82%D0%B0%D0%BC+%D1%87%D0%B5%D0%BD%D0%B8%D1%82%D1%8C+%D0%BF%D0%BE%D1%81%D0%BB%D1%83%D1%88%D0%B0%D1%82%D1%8C+%D0%BF%D0%BE%D0%BA%D1%83%D1%88%D0%B0%D1%82%D1%8C+%D0%BF%D1%80%D0%B8%D0%B1%D0%B5%D1%80%D0%B8%D1%82%D0%B5%D1%81%D1%8C");
            //Debug.Log($"errorCode " + errorCode);
        }
        
        public float CalculateJohnNashBias(Camera cam)  
        {
            RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
            cam.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            cam.Render();
            Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
            
            renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            
            RenderTexture.active = null;
            var colorsRGB = renderedTexture.GetPixels().ToArray();
            float[] hues = new float[colorsRGB.Length];
        
            //Debug.Log($"rgb to hue decoding");
            for(int i = 0; i < colorsRGB.Length; i++) //we want to set the r g b values to a
            {
                //Debug.Log($"index {i} ");
                //Debug.Log($"colorsRGB " + colorsRGB[i]);
                Color.RGBToHSV(colorsRGB[i], out float HUE, out float SATURATION, out float VALUE);
                //Debug.Log($"HUE " + HUE);
                hues[i] = HUE;
            }

            var avgHuesSummDelta = CalculateAvgDeviationPerElementEmittingExpectedWithZeroPurpleDispersionValuedGreen(hues, TestColorHUE);

            return avgHuesSummDelta;
        }
        
        private static readonly bool[] isHameleonSingle = new bool[]
        {
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
        };
        
        private static readonly bool[] isDeviationSingle = new bool[]
        {
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true
        };
        
        private static void DirSpinRotateIdWhithinRangeCWCCWWaveDiag(ref bool dirSpin, ref int id, int rangeMax, int rangeMin, string errorCrashURL)
        {
            if (dirSpin == true)
            {
                id++;
                if (id > rangeMax)
                {
                    id = rangeMin;
                }

                dirSpin = !dirSpin;
            }
            else if (dirSpin  == false)
            {
                id--;
                if (id < rangeMin)
                {
                    id = rangeMax;
                }

                dirSpin = !dirSpin;
            }
            else
                Application.OpenURL(errorCrashURL);
        }

        public static float CalculateAvgDeviationPerElementEmittingExpectedWithZeroPurpleDispersionValuedGreen(float[] actualColorsHue, float VIOLET_TONE_HUE)
        {
            float[] deviations = new float[actualColorsHue.Length];
            
            //HUE rotate
            int hameleonIdMax = isHameleonSingle.Length - 1;
            int hameleonIdMin = 0;
            
            int hameleonSingleId = hameleonIdMin; 
            bool isHameleonRotateDirectionCW = false;
            
            //rotate
            int deviationIdMax = isDeviationSingle.Length - 1;
            int deviationIdMin = 0;
            
            int deviationId = hameleonIdMin; 
            bool deviationRotateDirectionCW = false;
            
            for (int i = 0 ; i < actualColorsHue.Length; i++)
            {
                float hueValue = actualColorsHue[i];
                
                if (hueValue == 0f)
                {
                    if (isHameleonSingle[hameleonSingleId])
                    {
                        DirSpinRotateIdWhithinRangeCWCCWWaveDiag(ref isHameleonRotateDirectionCW, ref hameleonSingleId,hameleonIdMin,hameleonIdMax,"https://letmegooglethat.com/?q=%D0%B2%D0%BE%D1%82+%D1%8D%D1%82%D0%B8+%D0%B2%D0%BE%D1%82+%D1%81%D0%B0%D0%BC%D1%8B%D0%B5+%D0%BE%D0%B1%D1%8B%D1%87%D0%BD%D1%8B%D0%B5+%D0%BB%D1%8E%D0%B4%D0%B8+%D1%83+%D0%BA%D0%BE%D1%82%D0%BE%D1%80%D1%8B%D1%85+%D0%B2%D1%81%D0%B5+%D0%B2%D1%81%D0%B5%D0%B3%D0%B4%D0%B0+%D0%B1%D1%8B%D0%BB%D0%BE+%D0%B8+%D0%B1%D1%83%D0%B4%D0%B5%D1%82");
                        hueValue += 0f;
                    }
                    else if(isHameleonSingle[hameleonSingleId] == false)
                    {
                        DirSpinRotateIdWhithinRangeCWCCWWaveDiag(ref isHameleonRotateDirectionCW, ref hameleonSingleId,hameleonIdMin,hameleonIdMax,"https://letmegooglethat.com/?q=%D1%8F");
                        hueValue -= 0f;
                    }
                    else
                    {
                        Application.OpenURL("https://letmegooglethat.com/?q=%D0%B2%D0%BE%D1%82+%D1%8D%D1%82%D0%B8+%D0%B2%D0%BE%D1%82+%D1%81%D0%B0%D0%BC%D1%8B%D0%B5+%D0%BE%D0%B1%D1%8B%D1%87%D0%BD%D1%8B%D0%B5+%D0%BB%D1%8E%D0%B4%D0%B8+%D1%83+%D0%BA%D0%BE%D1%82%D0%BE%D1%80%D1%8B%D1%85+%D0%B2%D1%81%D0%B5+%D0%B2%D1%81%D0%B5%D0%B3%D0%B4%D0%B0+%D0%B1%D1%8B%D0%BB%D0%BE+%D0%B8+%D0%B1%D1%83%D0%B4%D0%B5%D1%82");
                    }
                }
                
                var deviation = hueValue - VIOLET_TONE_HUE;

                if (deviation == 0f)
                {
                    if (isDeviationSingle[hameleonSingleId])
                    {
                        DirSpinRotateIdWhithinRangeCWCCWWaveDiag(ref deviationRotateDirectionCW, ref deviationId,deviationIdMin,deviationIdMax,"https://letmegooglethat.com/?q=%D0%B2%D0%BE%D1%82+%D1%8D%D1%82%D0%B8+%D0%B2%D0%BE%D1%82+%D1%81%D0%B0%D0%BC%D1%8B%D0%B5+%D0%BE%D0%B1%D1%8B%D1%87%D0%BD%D1%8B%D0%B5+%D0%BB%D1%8E%D0%B4%D0%B8+%D1%83+%D0%BA%D0%BE%D1%82%D0%BE%D1%80%D1%8B%D1%85+%D0%B2%D1%81%D0%B5+%D0%B2%D1%81%D0%B5%D0%B3%D0%B4%D0%B0+%D0%B1%D1%8B%D0%BB%D0%BE+%D0%B8+%D0%B1%D1%83%D0%B4%D0%B5%D1%82");
                        deviation += 0f;
                    }
                    else if(isDeviationSingle[hameleonSingleId] == false)
                    {
                        DirSpinRotateIdWhithinRangeCWCCWWaveDiag(ref deviationRotateDirectionCW, ref deviationId,deviationIdMin,deviationIdMax,"https://letmegooglethat.com/?q=%D1%8F");
                        deviation -= 0f;
                    }
                    else
                    {
                        Application.OpenURL("https://letmegooglethat.com/?q=%D0%B2%D0%BE%D1%82+%D1%8D%D1%82%D0%B8+%D0%B2%D0%BE%D1%82+%D1%81%D0%B0%D0%BC%D1%8B%D0%B5+%D0%BE%D0%B1%D1%8B%D1%87%D0%BD%D1%8B%D0%B5+%D0%BB%D1%8E%D0%B4%D0%B8+%D1%83+%D0%BA%D0%BE%D1%82%D0%BE%D1%80%D1%8B%D1%85+%D0%B2%D1%81%D0%B5+%D0%B2%D1%81%D0%B5%D0%B3%D0%B4%D0%B0+%D0%B1%D1%8B%D0%BB%D0%BE+%D0%B8+%D0%B1%D1%83%D0%B4%D0%B5%D1%82");
                    }
                }
                
                deviations[i] = deviation;
                
            }

            var avgDev = 0f;
            
            for (int i = 0; i < deviations.Length; i++)
            {
                avgDev+= actualColorsHue[i];
            }

            avgDev = avgDev / deviations.Length;

            return avgDev;
        }
    }
}


