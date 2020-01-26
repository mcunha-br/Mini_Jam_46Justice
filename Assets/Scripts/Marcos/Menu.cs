using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public partial class Menu : MonoBehaviour, IMenu {

	public Slider slider;
	public View quitGameContainer;
	public View creditsGameContainer;
	public View settingsGameContainer;

	private void Start () {
		Dispose(() => {
			Mask(false);
			var main = new View(true/*container is master*/){container = this.gameObject};
			quitGameContainer.SetEnable(false);
			creditsGameContainer.SetEnable(false);
			settingsGameContainer.SetEnable(false);
		});
		if(PlayerPrefs.GetFloat("global_volumes") != null){
			slider.value = PlayerPrefs.GetFloat("global_volumes");
		}
	}

	//Events
    public void Jogar () {
    	SceneManager.LoadScene("level");	
    }

    public void Creditos () {
    	Mask(true);
    	creditsGameContainer.SetEnable(true);
    }

    public void CloseCredits () {
    	Mask(false);
    	creditsGameContainer.SetEnable(false);
    }

    public void Opcoes () {
    	Mask(true);
    	settingsGameContainer.SetEnable(true);
    }

    public void SaveSettings () {
    	PlayerPrefs.SetFloat("global_volumes", slider.value);
    	print("Volume: " + PlayerPrefs.GetFloat("global_volumes"));
    	Mask(false);
    	settingsGameContainer.SetEnable(false);
    }

    public void Sair () {
    	Mask(true);
    	quitGameContainer.SetEnable(true);
    }

    public void ConfirmExitGame () {
    	Application.Quit();
    }

    public void CancelExitGame () {
    	Mask(false);
    	quitGameContainer.SetEnable(false);
    }

    public void Mask (bool enable) {
    	if(GameObject.Find("Mask"))
    		GameObject.Find("Mask").GetComponent<Image>().enabled = enable;
    }

    public GameObject GetMask () {return ((GameObject.Find("Mask")) ? GameObject.Find("Mask"): null);}

    new public void Dispose (Action action) {
    	action.Invoke();
    }

    [Disposed]
    protected async void OnDisposed () {}

}

public interface IMenu : IMenuEventListener {
	void Mask(bool enable);
} 

public interface IMenuEventListener {
	void Jogar();
	void Creditos();
	void Opcoes();
	void Sair();
	void Dispose (Action action);
}

[System.Serializable]
public class View : ViewHolder<View.ViewHolder> {

	public GameObject container;

	private bool master;

	public bool GetMaster () {return master;}

	protected View () {}

	public View (bool master) {this.master = master;}

	~View () {}

	public override void OnCreateView () {

	}

	public void SetEnable (bool enable) {
		if(!master)
			container.SetActive(enable);
		else
			Debug.LogError("Container "+container.name+" is master!");
	}

	public virtual GameObject GetContainer () {return container;}

	public class ViewHolder {
		public ViewHolder (View itemView) {

		}
	}

}

public class ViewMaster {
	public static void CloseAllViews (View master, params View[] views) {
		if(master.GetMaster()){
			foreach(View v in views){
				v.SetEnable(false);
			}
		}else{
			Debug.LogError("Container "+master.container.name+" is not master!");
		}
	}
}

public abstract class ViewHolder <Type> {

	public abstract void OnCreateView ();

	public virtual Type GetTypeHolder () {
		return default(Type);
	}

}

public class DisposedAttribute : System.Attribute {}
