using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using GPG220.Luca.Scripts.Resources;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

namespace GPG220.Luca.Scripts.Editor
{
    public class UnitBaseWorldEditor : EditorWindow
    {
        private Vector2 unitListScrollPos = Vector2.zero;
        private Vector2 infoListScrollPos = Vector2.zero;
        
        /*
        float currentUnitListScrollViewWidth;
        float currentInfoListScrollViewWidth;*/
        
        bool resize = false;
        Rect cursorChangeRect;

        private Vector2 lastWindowSize = Vector2.zero;

        private UnitBase[] allUnits;
        private UnitBase[] selectedUnits;
        private UnitBase[] filteredUnits;
        
        [MenuItem("Tools/UnitBase World Editor")]
        public static void ShowWindow()
        {
            GetWindow<UnitBaseWorldEditor>(false, "UnitBase World Editor", true);
        }

        private Rect headerAreaRect;
        private Rect unitListAreaRect;
        private Rect infoListAreaRect;
        private Rect footerAreaRect;
        
        public Vector2 unitListAreaMinSize = new Vector2(400,100);
        public Vector2 unitListAreaMaxSize = new Vector2(-1,-1);
        public Vector2 infoListAreaMinSize = new Vector2(200,100);
        public Vector2 infoListAreaMaxSize = new Vector2(-1,-1);

        private string[] unitBaseSubTypes;
        
        private List<ResourceType> _resourceTypes = new List<ResourceType>();
        
        // Filter Selections
        private LayerMask unitBaseSubTypeMask = -1;
        private SelectionFilter selectionFilter = SelectionFilter.AllUnits;


        //public float changeRectWidth = 5f;
     
        private enum SelectionFilter
        {
            AllUnits, SelectedUnits
        }

        void OnEnable()
        {
            LoadUnitBaseSubTypes();
            LoadResourceTypes();
            position = new Rect(200,200,800,400);
            lastWindowSize = new Vector2(800,400);
            

            unitListAreaMaxSize.x = position.width - infoListAreaMinSize.x;
            unitListAreaMaxSize.y = position.height - headerAreaRect.height - footerAreaRect.height;
            infoListAreaMaxSize.x = position.width - unitListAreaMinSize.x;
            infoListAreaMaxSize.y = unitListAreaMaxSize.y;
            
            SetUnitListAreaSize(position.width/5 * 3);
            //SetInfoListAreaSize(position.width/3);
            
            /*
            currentUnitListScrollViewWidth = position.width/3 * 2;
            currentInfoListScrollViewWidth = position.width/3;

            unitListAreaRect.width = currentUnitListScrollViewWidth;
            infoListAreaRect.width = currentInfoListScrollViewWidth;*/
            
            //cursorChangeRect = new Rect(unitListAreaRect.width,unitListAreaRect.y,changeRectWidth,unitListAreaRect.height);
            
            OnWindowResize();
            OnSelectionChange();
            UpdateFilteredList();
        }

        private void LoadResourceTypes()
        {
            _resourceTypes = ResourceType.GetAllResources();
        }

        // TODO Hacky
        private void LoadUnitBaseSubTypes()
        {
            //unitBaseSubTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(UnitBase))).Select(type => type.Name).ToArray();
            unitBaseSubTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(UnitBase))).Select(type => type.Name).ToArray();

            //unitBaseSubTypes = (new string[] {"All Unit Types"}).Union(unitBaseSubTypes).ToArray();
        }
        

        private void Awake()
        {
            EditorApplication.playModeStateChanged += OnPlayModeState;
        }

        private void OnPlayModeState(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    LoadAllUnits();
                    LoadSelectedUnits();
                    UpdateFilteredList();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }


        private void OnGUI()
        {
            if(CheckWindowSizeChanged())
                OnWindowResize();

            
            EditorGUILayout.BeginVertical();
            DrawHeader();
            
            // Content
            EditorGUILayout.BeginHorizontal();
            
            DrawUnitList();
            
            //GUI.DrawTexture(cursorChangeRect,EditorGUIUtility.whiteTexture);
            
            DrawInfoList();
            //ResizeScrollView();
            
            EditorGUILayout.EndHorizontal();

            DrawFooter();
            
            EditorGUILayout.EndVertical();
            
            Repaint(); //TODO used?
            CaptureUnitSelection();
            
            lastWindowSize.x = position.width;
            lastWindowSize.y = position.height;
        }

        private void DrawFooter()
        {
            ////// Footer Start
            footerAreaRect = EditorGUILayout.BeginVertical();
            
            EditorGUILayout.LabelField("Lower part");
            
            ////// Footer End
            EditorGUILayout.EndVertical();
        }

        private void UpdateFilteredList()
        {
            List<UnitBase> newList;
            if (selectionFilter == SelectionFilter.SelectedUnits)
            {
                newList = new List<UnitBase>(selectedUnits);
            }else
            {
                if(allUnits == null || allUnits.Length == 0)
                    LoadAllUnits();
                newList = new List<UnitBase>(allUnits);
            }

            if (unitBaseSubTypeMask == 0)
            {
                newList = null;
            }else if (unitBaseSubTypeMask != -1)
            {
                List<string> layerNames = new List<string>();
                for (int i = 0; i < 32; i++)
                {
                    if (unitBaseSubTypeMask == (unitBaseSubTypeMask | (1 << i)))
                    {
                        layerNames.Add(unitBaseSubTypes[i]);
                    }
                }

                if (layerNames.Count > 0 && newList.Count > 0)
                {
                    for (int i = newList.Count-1; i >= 0; i--)
                    {
                        var type = newList[i].GetType().Name;
                        if (!layerNames.Contains(type))
                        {
                            newList.RemoveAt(i);
                        }
                    }
                }
            }
            

            filteredUnits = newList?.ToArray();
        }

        private int selectedIndex = -1;

        private readonly Color32 _listElementEvenColor = new Color32(230,230,230,255);
        private readonly Color32 _listElementUnevenColor = new Color32(150,150,150,255);
        private readonly Color32 _listElementHighlightedColor = new Color32(200,200,0,255);

        private readonly Dictionary<int,Rect> _unitEntryRects = new Dictionary<int,Rect>();
        private void DrawUnitListElement(UnitBase unit, int index = 0)
        {
            var style = new GUIStyle {};
            //style.normal.background = MakeTex((int)unitListAreaRect.width, (int)unitListAreaRect.height, infoBoxBackgroundColor);
            var r = EditorGUILayout.BeginHorizontal(style);
            GUI.DrawTexture(r, MakeTex((int)unitListAreaRect.width, (int)unitListAreaRect.height, (selectedIndex == index ? _listElementHighlightedColor : (index%2 == 0 ? _listElementEvenColor :_listElementUnevenColor))), ScaleMode.StretchToFill);
            EditorGUILayout.LabelField("Player [X]", GUILayout.MaxWidth(80));
            EditorGUILayout.LabelField("("+unit.GetType().Name+")", GUILayout.MaxWidth(150));

            var health = unit.GetComponent<Health>();
            EditorGUILayout.LabelField("Health: "+(health != null?"[X]/[Max]":"N/A"), GUILayout.MaxWidth(130)); // TODO No access to health currently...
            
            if (GUILayout.Button("Go To", EditorStyles.miniButtonLeft))
            {
                EditorGUIUtility.PingObject(unit);
                var activeSelection = Selection.activeGameObject;
                Selection.activeGameObject = unit.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();
                
                Selection.activeGameObject = activeSelection;
            }
            if (GUILayout.Button("Follow", EditorStyles.miniButtonMid))
            {
                Selection.activeGameObject = unit.gameObject;
                SceneView.FrameLastActiveSceneViewWithLock();
            }

            if (health == null)
                GUI.enabled = false;
            GUIStyle killBtnStyle = new GUIStyle(EditorStyles.miniButtonRight);
            killBtnStyle.normal.background = MakeTex((int) unitListAreaRect.width, (int) unitListAreaRect.height,
                new Color32(150, 0, 0, 255));
            killBtnStyle.normal.textColor = Color.white;
            killBtnStyle.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("Kill", killBtnStyle) && health != null)
            {
                health.ChangeHealth(health.startingHealth * -10); // TODO Can't get current health due to visibility ....
            }
            GUI.enabled = true;
                
            
            EditorGUILayout.EndHorizontal();
            _unitEntryRects.Add(index, r);
        }

        private void DrawHeader()
        {
            ////// Header Start
            headerAreaRect = EditorGUILayout.BeginVertical();

            var titleStyle = new GUIStyle {fontSize = 20, fontStyle = FontStyle.Bold};
            EditorGUILayout.LabelField("Unit Base World Editor",titleStyle);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            DrawUnitFilters();
            
            ////// Header End
            EditorGUILayout.EndVertical();
        }

        
        private void DrawUnitFilters()
        {
            EditorGUILayout.BeginVertical();
            // Selection Filter
            EditorGUILayout.BeginHorizontal ();
            GUIStyle gs = new GUIStyle(EditorStyles.miniButtonLeft);
            if (selectionFilter == SelectionFilter.AllUnits)
                gs.normal = EditorStyles.miniButtonLeft.active;
            else
                gs.normal = EditorStyles.miniButtonLeft.normal;
            if (GUILayout.Button ("All Units", gs))
            {
                selectionFilter = SelectionFilter.AllUnits;
                UpdateFilteredList();
            }
            GUIStyle gs2 = new GUIStyle(EditorStyles.miniButtonRight);
            if(selectionFilter == SelectionFilter.SelectedUnits)
                gs2.normal = EditorStyles.miniButtonRight.active;
            else
                gs2.normal = EditorStyles.miniButtonRight.normal;
            if (GUILayout.Button ("Selected", gs2)) {
                selectionFilter = SelectionFilter.SelectedUnits;
                UpdateFilteredList();
            }
            EditorGUILayout.EndHorizontal ();
            
            EditorGUILayout.Space();
            
            // Unit Type Filter
            /*int selected = 0;
            selected = EditorGUILayout.Popup("Unit Types", selected, unitBaseSubTypes);*/
            LayerMask newUnitBaseSubTypeMask = EditorGUILayout.MaskField("Unit Types",unitBaseSubTypeMask, unitBaseSubTypes);
            if (!newUnitBaseSubTypeMask.Equals(unitBaseSubTypeMask))
            {
                unitBaseSubTypeMask = newUnitBaseSubTypeMask;
                UpdateFilteredList();
            }
            
            EditorGUILayout.Space();
            
            // Load Btn
            if (GUILayout.Button("Refresh List"))
            {
                LoadAllUnits();
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            EditorGUILayout.EndVertical();
        }

        private void DrawUnitList()
        {
            var gStyle = new GUIStyle {/*padding = infoBoxPadding*/};
            gStyle.margin.left = infoBoxPaddingF;
            gStyle.margin.right = infoBoxPaddingF;
            gStyle.margin.top = infoBoxPaddingF;
            gStyle.margin.bottom = infoBoxPaddingF;
            //gStyle.stretchHeight = true;
            var viewOptions = new GUILayoutOption[2]{GUILayout.ExpandHeight(true), GUILayout.Width(unitListAreaRect.width)};
            var newListAreaRect = EditorGUILayout.BeginVertical(gStyle,viewOptions);
            
            
             
            var scrollViewOptions = new GUILayoutOption[2]{GUILayout.ExpandHeight(true), GUILayout.Width(unitListAreaRect.width)};
            
            
            var sStyle = new GUIStyle {/*padding = infoBoxPadding*/};
            //sStyle.normal.background = MakeTex((int)unitListAreaRect.width, (int)unitListAreaRect.height, infoBoxBackgroundColor);
            unitListScrollPos = GUILayout.BeginScrollView(unitListScrollPos,sStyle,scrollViewOptions);
            /////////////// UNIT LIST CONTENT START

            _unitEntryRects.Clear();
            if (filteredUnits != null && filteredUnits.Length > 0)
            {
                var currentActualIndex = 0;
                for (int i = 0; i < filteredUnits.Length; i++)
                {
                    if (allUnits[i] != null)
                    {
                        DrawUnitListElement(filteredUnits[i],currentActualIndex);
                        currentActualIndex++;
                    }
                        
                }
            }
            
            /////////////// UNIT LIST CONTENT END
            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            //var newListAreaRect = EditorGUILayout.GetControlRect();
            unitListAreaRect.x = newListAreaRect.x;
            unitListAreaRect.y = newListAreaRect.y;
            //unitListAreaRect = newListAreaRect;
        }

        private void DrawInfoList()
        {
            var gStyle = new GUIStyle {/*padding = infoBoxPadding*/};
            gStyle.margin.left = infoBoxPaddingF;
            gStyle.margin.right = infoBoxPaddingF;
            gStyle.margin.top = infoBoxPaddingF;
            gStyle.margin.bottom = infoBoxPaddingF;
            
            //gStyle.stretchHeight = true;
            var viewOptions = new GUILayoutOption[2]{GUILayout.ExpandHeight(true), GUILayout.Width(infoListAreaRect.width)};
            var newInfoListAreaRect = EditorGUILayout.BeginVertical(gStyle, viewOptions);
            var scrollViewOptions = new GUILayoutOption[2]{GUILayout.ExpandHeight(true), GUILayout.Width(infoListAreaRect.width)}; //GUILayout.Width(infoListAreaRect.width-2*infoBoxPaddingF), GUILayout.Height(infoListAreaRect.height), 
            infoListScrollPos = GUILayout.BeginScrollView(infoListScrollPos,scrollViewOptions);
            /////////////// INFO CONTENT START
            if (filteredUnits != null && filteredUnits.Length > 0)
            {
                if (filteredUnits.Length == 1 || selectedIndex >= 0)
                {
                    var filteredUnitIndex = (selectedIndex < 0 || selectedIndex >= filteredUnits.Length) ? (filteredUnits.Length == 0 ? -1 : 0) : selectedIndex;
                    if (filteredUnitIndex >= 0)
                    {
                        var unit = filteredUnits[filteredUnitIndex];
                        if (unit != null)
                        {
                            DrawInfoBoxSingle(unit);
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();
                            DrawInfoBoxInventory(unit);
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();
                            DrawInfoBoxUnitComponent(unit);
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();
                        }
                        
                    }
                    
                }else if (filteredUnits.Length > 1)
                {
                    DrawInfoBoxMultiple();
                }
            }
            /////////////// INFO CONTENT END
            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            /*ar newListAreaRect = EditorGUILayout.GetControlRect();*/
            infoListAreaRect.x = newInfoListAreaRect.x;//position.width - newListAreaRect.width;
            infoListAreaRect.y = newInfoListAreaRect.y;
            //infoListAreaRect = newInfoListAreaRect;
        }

        private void DrawInfoBoxUnitComponent(UnitBase unit)
        {
            if (unit == null) return;
            var unitHealth = unit?.GetComponent<Health>();
            
            var infoBoxStyle = new GUIStyle {/*padding = infoBoxPadding*/};
            infoBoxStyle.padding.left = infoBoxPaddingF;
            infoBoxStyle.padding.right = infoBoxPaddingF;
            infoBoxStyle.padding.top = infoBoxPaddingF;
            infoBoxStyle.padding.bottom = infoBoxPaddingF;
            
            var r = EditorGUILayout.BeginVertical(infoBoxStyle);
            GUI.DrawTexture(r, MakeTex((int)unitListAreaRect.width, (int)unitListAreaRect.height, infoBoxBackgroundColor), ScaleMode.StretchToFill);
            
            var titleStyle = new GUIStyle {fontSize = 15, fontStyle = FontStyle.Bold};
            EditorGUILayout.LabelField("Unit Component",titleStyle);
            
            var editor = UnityEditor.Editor.CreateEditor( unit );
            editor.DrawDefaultInspector();
            
            GUILayout.BeginArea(r);
            editor.DrawDefaultInspector(); //confines the editor to the specified rectangle
            GUILayout.EndArea();
            
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        private Color32 infoBoxBackgroundColor = new Color32(150,150,150,255);
        private int infoBoxPaddingF = 5;

        private void DrawInfoBoxSingle(UnitBase unit)
        {
            if (unit == null) return;
            var unitHealth = unit?.GetComponent<Health>();
            
            var infoBoxStyle = new GUIStyle {/*padding = infoBoxPadding*/};
            infoBoxStyle.padding.left = infoBoxPaddingF;
            infoBoxStyle.padding.right = infoBoxPaddingF;
            infoBoxStyle.padding.top = infoBoxPaddingF;
            infoBoxStyle.padding.bottom = infoBoxPaddingF;
            
            var r = EditorGUILayout.BeginVertical(infoBoxStyle);
            GUI.DrawTexture(r, MakeTex((int)unitListAreaRect.width, (int)unitListAreaRect.height, infoBoxBackgroundColor), ScaleMode.StretchToFill);
            //GUI.backgroundColor = infoBoxBackgroundColor;

            var titleStyle = new GUIStyle {fontSize = 15, fontStyle = FontStyle.Bold};
            EditorGUILayout.LabelField("Unit Overview",titleStyle);
            EditorGUILayout.LabelField("Player:","[X]");
            EditorGUILayout.LabelField("Type:",unit.GetType().Name);
            EditorGUILayout.LabelField("Health:","[X]/[Max]");


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select", EditorStyles.miniButtonLeft))
            {
                Selection.activeGameObject = unit.gameObject;
                if (SceneView.sceneViews.Count > 0)
                {
                    var sceneView = (SceneView)SceneView.sceneViews[0];
                    sceneView.Focus();
                }
            }
            if (GUILayout.Button("Go To", EditorStyles.miniButtonMid))
            {
                EditorGUIUtility.PingObject(unit);
                var activeSelection = Selection.activeGameObject;
                Selection.activeGameObject = unit.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();
                
                Selection.activeGameObject = activeSelection;
            }
            if (GUILayout.Button("Follow", EditorStyles.miniButtonMid))
            {
                Selection.activeGameObject = unit.gameObject;
                SceneView.FrameLastActiveSceneViewWithLock();
            }
            GUIStyle killBtnStyle = new GUIStyle(EditorStyles.miniButtonRight);
            killBtnStyle.normal.background = MakeTex((int) unitListAreaRect.width, (int) unitListAreaRect.height,
                new Color32(150,0,0,255));
            killBtnStyle.normal.textColor = Color.white;
            killBtnStyle.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("Kill", killBtnStyle))
            {
                if (unitHealth == null)
                    return;
                unitHealth.ChangeHealth(unitHealth.startingHealth * -10); // TODO Can't get current health due to visibility ....
            }
            EditorGUILayout.EndHorizontal();
            
            
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        private int selectedResourceIndex = 0;
        private int selectedResourceAmount = 1;
        private void DrawInfoBoxInventory(UnitBase unit)
        {
            var unitInventory = unit?.inventory ?? unit?.GetComponent<Inventory>();
            if (unit == null || unitInventory == null) return;
            
            var infoBoxStyle = new GUIStyle {/*padding = infoBoxPadding*/};
            infoBoxStyle.padding.left = infoBoxPaddingF;
            infoBoxStyle.padding.right = infoBoxPaddingF;
            infoBoxStyle.padding.top = infoBoxPaddingF;
            infoBoxStyle.padding.bottom = infoBoxPaddingF;
            
            var r = EditorGUILayout.BeginVertical(infoBoxStyle);
            GUI.DrawTexture(r, MakeTex((int)unitListAreaRect.width, (int)unitListAreaRect.height, infoBoxBackgroundColor), ScaleMode.StretchToFill);
            //GUI.backgroundColor = infoBoxBackgroundColor;

            var titleStyle = new GUIStyle {fontSize = 15, fontStyle = FontStyle.Bold};
            EditorGUILayout.LabelField("Unit Inventory",titleStyle);

            var resStyle = new GUIStyle {fontSize = 12, alignment = TextAnchor.MiddleLeft, clipping = TextClipping.Clip, stretchWidth = false, fontStyle = FontStyle.Bold};
            foreach (var kvp in unitInventory.GetResourceQuantities())
            {
                var entryRect = EditorGUILayout.BeginHorizontal();
                var x = EditorGUILayout.GetControlRect(GUILayout.MaxWidth(50), GUILayout.MaxHeight(50));
                if(kvp.Key.ResourceThumbnail != null)
                    GUI.DrawTexture(x, kvp.Key.ResourceThumbnail.texture,ScaleMode.ScaleToFit);
                else
                    GUI.DrawTexture(x, MakeTex(50, 50, Color.white), ScaleMode.ScaleToFit);
                
                EditorGUILayout.BeginVertical();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(kvp.Key.ResourceName, resStyle, GUILayout.MaxWidth(100));
                EditorGUILayout.LabelField(kvp.Value.ToString(), resStyle, GUILayout.MaxWidth(100));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-5", EditorStyles.miniButtonLeft))
                {
                    unitInventory.RemoveResources(kvp.Key, 5);
                }
                if (GUILayout.Button("-1", EditorStyles.miniButtonMid))
                {
                    unitInventory.RemoveResources(kvp.Key, 1);
                }
                if (GUILayout.Button("0", EditorStyles.miniButtonMid))
                {
                    unitInventory.RemoveResources(kvp.Key, kvp.Value);
                }
                if (GUILayout.Button("+1", EditorStyles.miniButtonMid))
                {
                    unitInventory.AddResources(kvp.Key, 1);
                }
                if (GUILayout.Button("+5", EditorStyles.miniButtonRight))
                {
                    unitInventory.AddResources(kvp.Key, 5);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            var addResRect = EditorGUILayout.BeginHorizontal(infoBoxStyle);
            GUI.DrawTexture(addResRect, MakeTex((int)unitListAreaRect.width, (int)unitListAreaRect.height, new Color32(99,99,99,255)), ScaleMode.StretchToFill);
            
            //EditorGUILayout.LabelField("Add", resStyle);
            selectedResourceIndex = EditorGUILayout.IntPopup(selectedResourceIndex, _resourceTypes.Select(res => res.ResourceName).ToArray(),_resourceTypes.Select((res,i) => i).ToArray());
            selectedResourceAmount = EditorGUILayout.IntField(selectedResourceAmount);
            
            if (GUILayout.Button("Add") && selectedResourceIndex >= 0 && selectedResourceAmount > 0 && _resourceTypes.Count > selectedResourceIndex)
            {
                unitInventory.AddResources(_resourceTypes[selectedResourceIndex], selectedResourceAmount);
            }
            
            
            EditorGUILayout.EndHorizontal();
            /*

            if (GUILayout.Button("Kill Unit"))
            {
                if (unitHealth == null)
                    return;
                unitHealth.ChangeHealth(unitHealth.startingHealth * -10); // TODO Can't get current health due to visibility ....
            }*/
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }
        
        private void DrawInfoBoxMultiple()
        {
            var infoBoxStyle = new GUIStyle {/*padding = infoBoxPadding*/};
            infoBoxStyle.padding.left = infoBoxPaddingF;
            infoBoxStyle.padding.right = infoBoxPaddingF;
            infoBoxStyle.padding.top = infoBoxPaddingF;
            infoBoxStyle.padding.bottom = infoBoxPaddingF;
            //var drawInfoBoxMultipleRect = EditorGUILayout.GetControlRect();
            /*
            if(drawInfoBoxMultipleRect.width > 2*infoBoxPaddingF && drawInfoBoxMultipleRect.height > 2*infoBoxPaddingF)
                infoBoxStyle.normal.background = MakeTex((int)drawInfoBoxMultipleRect.width-(2*infoBoxPaddingF), (int)drawInfoBoxMultipleRect.height-(2*infoBoxPaddingF), infoBoxBackgroundColor);*/
            //infoBoxStyle.normal.background = MakeTex((int)drawInfoBoxMultipleRect.width, (int)drawInfoBoxMultipleRect.height, infoBoxBackgroundColor);
            var r = EditorGUILayout.BeginVertical(infoBoxStyle);
            GUI.DrawTexture(r, MakeTex((int)unitListAreaRect.width, (int)unitListAreaRect.height, infoBoxBackgroundColor), ScaleMode.StretchToFill);
            //GUI.backgroundColor = infoBoxBackgroundColor;

            var titleStyle = new GUIStyle {fontSize = 15, fontStyle = FontStyle.Bold};
            EditorGUILayout.LabelField("Unit Types",titleStyle);
            
            var units = new Dictionary<string, int>();
            filteredUnits.ForEach(unit =>
            {
                var unittype = unit.GetType().Name;

                if (!units.ContainsKey(unittype))
                {
                    units.Add(unittype,1);
                }
                else
                {
                    units[unittype] += 1;
                }
            });

            units.ForEach(entry =>
            {
                EditorGUILayout.LabelField(entry.Key, entry.Value.ToString());
            });
            
            GUIStyle killBtnStyle = new GUIStyle(EditorStyles.miniButtonRight);
            killBtnStyle.normal.background = MakeTex((int) unitListAreaRect.width, (int) unitListAreaRect.height,
                new Color32(150,0,0,255));
            killBtnStyle.normal.textColor = Color.white;
            killBtnStyle.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("Kill All Units",killBtnStyle))
            {
                filteredUnits.ForEach(unit =>
                {
                    if (unit == null) return;
                    var unitHealth = unit.GetComponent<Health>();
                    if (unitHealth == null) return;
                    unitHealth.ChangeHealth(unitHealth.startingHealth * -10); // TODO Can't get current health due to visibility ....
                });
            }
            
            EditorGUILayout.EndVertical();
        }

        private void OnSelectionChange()
        {
            LoadSelectedUnits();
        }

        public bool CheckWindowSizeChanged()
        {
            return !Mathf.Approximately( position.width, lastWindowSize.x) || !Mathf.Approximately(position.height, lastWindowSize.y);
        }

        public void OnWindowResize()
        {
            var widthChangeDelta = position.width / lastWindowSize.x;
            var heightChangeDelta = position.height / lastWindowSize.y;
            
            /*currentUnitListScrollViewWidth *= widthChangeDelta;
            currentInfoListScrollViewWidth *= widthChangeDelta;
            unitListAreaRect.width = currentUnitListScrollViewWidth;
            infoListAreaRect.width = currentInfoListScrollViewWidth;*/
            unitListAreaMaxSize.x = position.width - infoListAreaMinSize.x;
            unitListAreaMaxSize.y = position.height - headerAreaRect.height - footerAreaRect.height;
            infoListAreaMaxSize.x = position.width - unitListAreaMinSize.x;
            infoListAreaMaxSize.y = unitListAreaMaxSize.y;
            
            SetUnitListAreaSize(unitListAreaRect.width * widthChangeDelta, unitListAreaRect.height * heightChangeDelta);
            //SetInfoListAreaSize(infoListAreaRect.width * widthChangeDelta, infoListAreaRect.height * heightChangeDelta);
            
            cursorChangeRect.Set(unitListAreaRect.width,unitListAreaRect.y,cursorChangeRect.width,unitListAreaRect.height);
        }

        void SetUnitListAreaSize(float width = -1, float height = -1)
        {
            if (width < 0) width = unitListAreaRect.width;
            if (height < 0) height = unitListAreaRect.height;

            unitListAreaRect.width = Mathf.Clamp(width, unitListAreaMinSize.x, unitListAreaMaxSize.x >= 0 ? unitListAreaMaxSize.x : position.width);
            unitListAreaRect.height = Mathf.Clamp(height, unitListAreaMinSize.y, unitListAreaMaxSize.y >= 0 ? unitListAreaMaxSize.y : position.height);

            //infoListAreaRect.x = unitListAreaRect.width;
            SetInfoListAreaSize(position.width - unitListAreaRect.width-15); // -15 cuz of paddings/margins; hack
        }

        void SetInfoListAreaSize(float width = -1, float height = -1)
        {
            if (width < 0) width = infoListAreaRect.width;
            if (height < 0) height = infoListAreaRect.height;

            infoListAreaRect.width = Mathf.Clamp(width, infoListAreaMinSize.x, infoListAreaMaxSize.x >= 0 ? infoListAreaMaxSize.x : position.width);
            infoListAreaRect.height = Mathf.Clamp(height, infoListAreaMinSize.y, infoListAreaMaxSize.y >= 0 ? infoListAreaMaxSize.y : position.height);

            //infoListAreaRect.x = position.width-infoListAreaRect.width;
            
            /*
            if (infoListAreaRect.height > unitListAreaRect.height)
            {
                SetUnitListAreaSize(-1, infoListAreaRect.height);
            }*/
        }

        private void LoadSelectedUnits()
        {//TODO
            selectedUnits = Selection.GetFiltered<UnitBase>(SelectionMode.Unfiltered);
        }

        private void LoadAllUnits()
        {
            allUnits = UnityEngine.Resources.FindObjectsOfTypeAll<UnitBase>();
        }

        private void CaptureUnitSelection()
        {

            EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.Orbit);
                
            var mousePos = Event.current.mousePosition;

            if (Event.current.type == EventType.MouseUp)
            {
                if (unitListAreaRect.Contains(mousePos))
                {
                    var relMousePos = mousePos;
                    relMousePos.x -= unitListAreaRect.x;
                    relMousePos.y -= unitListAreaRect.y;
                    var index = _unitEntryRects.Where(kvp => kvp.Value.Contains(relMousePos)).Select((kvp)=>kvp.Key).DefaultIfEmpty(-1).FirstOrDefault();
                    selectedIndex = index == selectedIndex ? -1 : index;
                }
                else if(!infoListAreaRect.Contains(mousePos))
                {
                    //selectedIndex = -1;
                }
            }
            
        }
     
        private void ResizeScrollView(){
            
            EditorGUIUtility.AddCursorRect(cursorChangeRect,MouseCursor.ResizeHorizontal);
         
            var mousePos = Event.current.mousePosition;
            if( Event.current.type == EventType.MouseDown && cursorChangeRect.Contains(mousePos)){
                resize = true;
            }
            if(resize){
                /*currentUnitListScrollViewWidth = Event.current.mousePosition.x;
                currentInfoListScrollViewWidth = position.width - Event.current.mousePosition.x;
                cursorChangeRect.Set(currentUnitListScrollViewWidth,cursorChangeRect.y,cursorChangeRect.width,cursorChangeRect.height);*/
                if (mousePos.x >= unitListAreaMinSize.x && mousePos.x <= unitListAreaMaxSize.x &&
                    position.width - mousePos.x - cursorChangeRect.width >= infoListAreaMinSize.x &&
                    position.width - mousePos.x - cursorChangeRect.width <= infoListAreaMaxSize.x)
                {
                    Debug.Log("FRUCK");
                    SetUnitListAreaSize(mousePos.x);
                    //SetInfoListAreaSize(position.width - mousePos.x);
                    cursorChangeRect.Set(unitListAreaRect.width,unitListAreaRect.y,cursorChangeRect.width,cursorChangeRect.height);
                }
            }
            if(Event.current.type == EventType.MouseUp)
                resize = false;        
        }
        
        //https://forum.unity.com/threads/changing-the-background-color-for-beginhorizontal.66015/
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width*height];
 
            for(int i = 0; i < pix.Length; i++)
                pix[i] = col;
 
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
 
            return result;
        }
    }
}
