using UnityEngine;
using System.Collections.Generic;

	public enum PageType
	{
		None,
		MainMenuPage,
        InGamePage
	}

public class Game : MonoBehaviour {

    public static Game instance;
	public FFont myfont;
	
	private PageType _currentPageType = PageType.None;
	private Page _currentPage = null;
	
	private FStage _stage;
	
	void Start () {
		instance = this;
		Go.defaultEaseType = EaseType.Linear;
		Go.duplicatePropertyRule = DuplicatePropertyRuleType.RemoveRunningProperty;
        FutileParams fparams = new FutileParams(true, true, false, false);

		fparams.AddResolutionLevel(960.0f,	1.0f,	1.0f,	""); //iPhone

		fparams.origin = new Vector2(0.5f,0.5f);

        fparams.backgroundColor = new Color(18f / 100f, 13f / 100f, 28f / 100f, 1f);
				
		Futile.instance.Init (fparams);

        Futile.atlasManager.LoadImage("Atlases/Font");
        Futile.atlasManager.LoadImage("Atlases/title");
        Futile.atlasManager.LoadImage("Atlases/bg");

        Futile.atlasManager.LoadAtlas("Atlases/SpriteSheet");
        Futile.atlasManager.LoadFont("Minecraftia", "Atlases/Font", "Atlases/Font", 0, 0);

		
		_stage = Futile.stage;
		
		GoToPage(PageType.MainMenuPage);
		
		_stage.ListenForUpdate (HandleUpdate);
	}
	
	void HandleUpdate ()
	{
		
	}
	
	public void GoToPage (PageType pageType)
	{

		Page pageToCreate = null;

		if(pageType == PageType.MainMenuPage)
		{
			pageToCreate = new MainMenuPage();
		}

		if(pageType == PageType.InGamePage)
		{
			pageToCreate = new InGamePage();
		}

		if(pageToCreate != null)
		{
			_currentPageType = pageType;	

			if(_currentPage != null)
			{
				_stage.RemoveChild(_currentPage);
			}

			_currentPage = pageToCreate;
			_stage.AddChild(_currentPage);
			_currentPage.Start();
			
		}
	}
}