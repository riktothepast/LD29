using UnityEngine;
using System.Collections;

public class MainMenuPage: Page {
	FLabel inst,shadow;

	public MainMenuPage()
	{
		ListenForUpdate(HandleUpdate);
		ListenForResize(HandleResize);
        inst = new FLabel("Minecraftia", "- Press return to start -");
        inst.y = Futile.screen.halfHeight - 50;
        FSprite title = new FSprite("Atlases/title");
        title.width = Futile.screen.width;
        title.height = Futile.screen.height;
        AddChild(title);
        AddChild(inst);
	}
	
	override public void Start () {
       
	}

	protected void HandleUpdate () {

        inst.alpha = Mathf.PingPong(Time.time, 1);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Game.instance.GoToPage(PageType.InGamePage);
        }
	}

    private void HandlebOkButtonRelease(FButton button)
    {
        Debug.Log("click");
        Game.instance.GoToPage(PageType.InGamePage);
    }
	

	protected void HandleResize(bool wasOrientationChange){
		
	}
}
