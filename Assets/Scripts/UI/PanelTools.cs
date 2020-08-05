
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SimuUI
{
    public class PanelTools : PanelBase<PanelTools>, ISimuPanel
    {
        public Transform transform_menu;
        public MenuLV1[] menuButtons
        {
            get
            {
                MenuLV1[] list= GetComponentsInChildren<MenuLV1>();
                return list;
            }
        }

        string[] menuNames = new string[]
        {
        "Reset",
        "Elements",
        "Set Car Pose",
        "Settings",
        "Help",
        "Exit"
        };
        public UnityAction[] menuActions;

        public List<string[]> menusNames = new List<string[]>
    {
        new string[]{"All Reset","Car Pose Reset" },
        new string[]{"Add CarAI","Add Human", "Add Obstacle", "Remove All" }
    };
        public List<UnityAction[]> menusActions = new List<UnityAction[]>();

        public string[] menuName1 = new string[] { "All Reset", "Car Reset" };

        private MenuLV1 menuButtonSelected = null;
        public MenuLV1 MenuButtonSelected
        {
            get
            {
                return menuButtonSelected;
            }
            set
            {
                menuButtonSelected = value;
                if (menuButtonSelected != null)
                {
                    OpenSeletedMenu();
                }
            }
        }
        void Start()
        {
            CloseAllMenu();
        }
        public void OpenSeletedMenu()
        {
            foreach (MenuLV1 button in menuButtons)
            {
                button.SetMenuActive(button == MenuButtonSelected && !menuButtonSelected.isOpen);
            }
        }
        public void CloseAllMenu()
        {
            foreach (MenuLV1 button in menuButtons)
            {
                button.SetMenuActive(false);
            }
        }
        public void OpenGitURL()
        {
            Application.OpenURL("https://github.com/autocore-ai/autocore_pcu_doc/blob/master/docs/Simulation_autoware.md");
        }
        
        public override void SetPanelActive(bool value)
        {
            return;
        }
    }
}
