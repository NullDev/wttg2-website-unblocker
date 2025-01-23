using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000230 RID: 560
public class TheCloud : MonoBehaviour
{
	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060010CC RID: 4300 RVA: 0x0004F727 File Offset: 0x0004DB27
	public string MasterKey
	{
		get
		{
			return this.masterKey;
		}
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x0004F72F File Offset: 0x0004DB2F
	public void InvalidURL(out string ReturnURL)
	{
		ReturnURL = "localGame://NotFound/index.html";
		this.curWebPageDef = null;
		this.curWebsiteDef = null;
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x0004F748 File Offset: 0x0004DB48
	public bool SoftValidateURL(out string returnURL, string checkURL = "")
	{
		returnURL = "localGame://NotFound/index.html";
		checkURL = checkURL.Replace("http://", string.Empty);
		checkURL = checkURL.Replace("https://", string.Empty);
		checkURL = checkURL.Replace("www.", string.Empty);
		checkURL = checkURL.Replace("redirect", string.Empty);
		string[] array = checkURL.Split(new string[]
		{
			"/"
		}, StringSplitOptions.None);
		return array[0].Equals("game.local") || this.validDomains.Contains(array[0].ToLower());
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x0004F7EC File Offset: 0x0004DBEC
	public bool ValidateURL(out string returnURL, string checkURL = "")
	{
		returnURL = "localGame://NotFound/index.html";
		if (checkURL != string.Empty)
		{
			checkURL = checkURL.Replace("http://", string.Empty);
			checkURL = checkURL.Replace("www.", string.Empty);
			checkURL = checkURL.Replace("redirect", string.Empty);
			string[] array = checkURL.Split(new string[]
			{
				"/"
			}, StringSplitOptions.None);
			string key = array[0].Replace(".ann", string.Empty);
			this.curWebPageDef = null;
			this.curWebsiteDef = null;
			if (this.validDomains.Contains(array[0].ToLower()))
			{
				returnURL = "http://www." + checkURL;
				SteamSlinger.Ins.CheckStalkerURL(returnURL);
			}
			else if (array[0].Contains(".ann"))
			{
				if (this.wikiLookUp.ContainsKey(key))
				{
					int index = this.wikiLookUp[key];
					this.curWebsiteDef = this.wikis[index];
					returnURL = "localGame://" + this.wikis[index].DocumentRoot + "/" + this.wikis[index].HomePage.FileName;
					this.curWebPageDef = this.wikis[index].HomePage;
				}
				else if (this.websiteLookUp.ContainsKey(key))
				{
					int index2 = this.websiteLookUp[key];
					this.curWebsiteDef = this.Websites[index2];
					if (array.Length > 1)
					{
						if (array[1] != string.Empty)
						{
							if (array[1].ToLower() == this.Websites[index2].HomePage.FileName)
							{
								returnURL = "localGame://" + this.Websites[index2].DocumentRoot + "/" + this.Websites[index2].HomePage.FileName;
								this.curWebPageDef = this.Websites[index2].HomePage;
							}
							else
							{
								for (int i = 0; i < this.Websites[index2].SubPages.Count; i++)
								{
									if (array[1].ToLower() == this.Websites[index2].SubPages[i].FileName)
									{
										returnURL = "localGame://" + this.Websites[index2].DocumentRoot + "/" + this.Websites[index2].SubPages[i].FileName;
										this.curWebPageDef = this.Websites[index2].SubPages[i];
										i = this.Websites[index2].SubPages.Count;
									}
								}
							}
						}
						else
						{
							returnURL = "localGame://" + this.Websites[index2].DocumentRoot + "/" + this.Websites[index2].HomePage.FileName;
							this.curWebPageDef = this.Websites[index2].HomePage;
						}
					}
					else if (this.Websites[index2].isFake)
					{
						returnURL = "localGame://NotFound/index.html";
						this.curWebPageDef = null;
						this.curWebsiteDef = null;
						if (!this.rollCoolDownActive)
						{
							GameManager.HackerManager.RollHack();
							this.rollTimeStamp = Time.time;
							this.rollCoolDownActive = true;
						}
					}
					else
					{
						returnURL = "localGame://" + this.Websites[index2].DocumentRoot + "/" + this.Websites[index2].HomePage.FileName;
						this.curWebPageDef = this.Websites[index2].HomePage;
					}
					if (this.Websites[index2].HasWindow)
					{
						bool flag = true;
						switch (this.Websites[index2].WindowTime)
						{
						case WEBSITE_WINDOW_TIME.FIRST_QUARTER:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 0 && GameManager.TimeKeeper.GetCurrentGameMin() <= 15)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.SECOND_QUARTER:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 15 && GameManager.TimeKeeper.GetCurrentGameMin() <= 30)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.THRID_QUARTER:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 30 && GameManager.TimeKeeper.GetCurrentGameMin() <= 45)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.FOURTH_QUARTER:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 45 && GameManager.TimeKeeper.GetCurrentGameMin() <= 60)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.FIRST_HALF:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 0 && GameManager.TimeKeeper.GetCurrentGameMin() <= 30)
							{
								flag = false;
							}
							break;
						case WEBSITE_WINDOW_TIME.SECNOND_HALF:
							if (GameManager.TimeKeeper.GetCurrentGameMin() >= 30 && GameManager.TimeKeeper.GetCurrentGameMin() <= 60)
							{
								flag = false;
							}
							break;
						}
						if (flag)
						{
							returnURL = "localGame://NotFound/index.html";
							this.curWebPageDef = null;
							this.curWebsiteDef = null;
						}
					}
				}
			}
		}
		if (returnURL == "localGame://NotFound/index.html")
		{
			return false;
		}
		if (this.curWebsiteDef != null && this.websiteLookUp.ContainsKey(this.curWebsiteDef.PageURL))
		{
			if (!this.curWebsiteDef.WasVisted && this.curWebsiteDef.IsTapped && !DataManager.LeetMode)
			{
				this.KeyDiscoveredEvent.Execute();
			}
			this.curWebsiteDef.WasVisted = true;
			int index3 = this.websiteLookUp[this.curWebsiteDef.PageURL];
			this.myWebSitesData.Sites[index3].Visted = true;
			DataManager.Save<WebSitesData>(this.myWebSitesData);
		}
		if (this.curWebPageDef != null)
		{
			this.curWebPageDef.InvokePageEvent();
		}
		return true;
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x0004FE1C File Offset: 0x0004E21C
	public void ClearCurrentWebDeff()
	{
		this.curWebsiteDef = null;
		this.curWebPageDef = null;
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x0004FE2C File Offset: 0x0004E22C
	public WebPageDefinition GetCurrentWebPageDef()
	{
		return this.curWebPageDef;
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x0004FE34 File Offset: 0x0004E234
	public void GetCurrentPageSourceCode()
	{
		if (this.curWebsiteDef != null && this.curWebPageDef != null)
		{
			string text = this.curWebPageDef.PageHTML;
			bool doSetHTML = false;
			if (this.curWebPageDef != this.lastSourceWebPageDef)
			{
				this.lastSourceWebPageDef = this.curWebPageDef;
				doSetHTML = true;
				if (this.curWebsiteDef.HoldsSecondWikiLink && this.curWebsiteDef.HomePage == this.curWebPageDef)
				{
					int num = UnityEngine.Random.Range(Mathf.RoundToInt((float)text.Length * 0.1f), text.Length);
					string text2 = text.Substring(0, num);
					string text3 = text.Substring(num);
					text = string.Concat(new string[]
					{
						text2,
						"<!-- ",
						this.GetWikiURL(1),
						" -->",
						text3
					});
				}
				if (this.curWebPageDef.IsTapped && this.curWebPageDef.KeyDiscoverMode == KEY_DISCOVERY_MODES.SOURCE_CODE)
				{
					int num2 = UnityEngine.Random.Range(Mathf.RoundToInt((float)text.Length * 0.3f), text.Length);
					string text4 = text.Substring(0, num2);
					string text5 = text.Substring(num2);
					text = string.Concat(new string[]
					{
						text4,
						"<!-- ",
						(this.curWebPageDef.HashIndex + 1).ToString(),
						" - ",
						this.curWebPageDef.HashValue,
						" -->",
						text5
					});
				}
			}
			GameManager.BehaviourManager.SourceViewerBehaviour.ViewSourceCode(text, doSetHTML);
		}
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x0004FFE0 File Offset: 0x0004E3E0
	public bool TriggerBookMark()
	{
		if (!(this.curWebsiteDef != null) || !(this.curWebPageDef != null))
		{
			return false;
		}
		if (!this.bookmarks.ContainsKey(this.curWebPageDef.GetHashCode()))
		{
			string setURL = "http://" + this.curWebsiteDef.PageURL + ".ann/" + this.curWebPageDef.FileName;
			BookmarkData bookmarkData = new BookmarkData(this.curWebPageDef.PageName, setURL);
			this.bookmarks.Add(this.curWebPageDef.GetHashCode(), bookmarkData);
			this.myBookMarksData.BookMarks.Add(this.curWebPageDef.GetHashCode(), bookmarkData);
			GameManager.BehaviourManager.AnnBehaviour.AddBookmarkTab(this.curWebPageDef.GetHashCode(), bookmarkData);
			DataManager.Save<BookMarksData>(this.myBookMarksData);
			return true;
		}
		this.bookmarks.Remove(this.curWebPageDef.GetHashCode());
		this.myBookMarksData.BookMarks.Remove(this.curWebPageDef.GetHashCode());
		GameManager.BehaviourManager.AnnBehaviour.RemoveBookmarkTab(this.curWebPageDef.GetHashCode());
		DataManager.Save<BookMarksData>(this.myBookMarksData);
		return false;
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x0005011C File Offset: 0x0004E51C
	public bool CheckToSeeIfPageIsBookMarked()
	{
		return this.curWebsiteDef != null && this.curWebPageDef != null && this.bookmarks.ContainsKey(this.curWebPageDef.GetHashCode());
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x0005016B File Offset: 0x0004E56B
	public bool CheckIfSiteWasTapped()
	{
		return this.curWebPageDef != null && this.curWebPageDef.IsTapped;
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x00050193 File Offset: 0x0004E593
	public bool CheckIfWiki()
	{
		return this.curWebsiteDef != null && this.wikiLookUp.ContainsKey(this.curWebsiteDef.PageURL);
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x000501C8 File Offset: 0x0004E5C8
	public JSONNode BuildCurrentWiki()
	{
		List<JSONNode> list = new List<JSONNode>(20);
		if (this.curWebsiteDef != null && this.wikiLookUp.ContainsKey(this.curWebsiteDef.PageURL))
		{
			int index = this.wikiLookUp[this.curWebsiteDef.PageURL];
			for (int i = 0; i < this.wikiSites[index].Count; i++)
			{
				int index2 = this.wikiSites[index][i];
				string text = string.Concat(new string[]
				{
					this.Websites[index2].PageURL,
					"|",
					this.Websites[index2].PageTitle,
					"|",
					this.Websites[index2].PageDesc,
					"|"
				});
				if (this.Websites[index2].WasVisted)
				{
					text += "1";
				}
				else
				{
					text += "0";
				}
				list.Add(text);
			}
		}
		return new JSONNode(list);
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x00050300 File Offset: 0x0004E700
	public string GetWikiURL(int WikiIndex)
	{
		string result = string.Empty;
		if (this.wikis[WikiIndex] != null)
		{
			result = "http://" + this.wikis[WikiIndex].PageURL + ".ann";
		}
		return result;
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x0005034C File Offset: 0x0004E74C
	public void ForceKeyDiscover()
	{
		if (this.KeyDiscoveredEvent != null)
		{
			this.KeyDiscoveredEvent.Execute();
		}
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x00050364 File Offset: 0x0004E764
	private void prepTheMasterKey()
	{
		this.myMasterKeyData = DataManager.Load<MasterKeyData>(1010);
		if (this.myMasterKeyData == null)
		{
			this.myMasterKeyData = new MasterKeyData(1010);
			this.myMasterKeyData.Keys = new List<string>(8);
			for (int i = 0; i < 8; i++)
			{
				string item = MagicSlinger.MD5It(string.Concat(new object[]
				{
					Time.time.ToString(),
					UnityEngine.Random.Range(0, 99999).ToString(),
					":REFLECTSTUDIOS:",
					Time.deltaTime,
					":",
					i
				})).Substring(0, 12);
				this.myMasterKeyData.Keys.Add(item);
			}
		}
		this.keys = new List<string>(this.myMasterKeyData.Keys);
		for (int j = 0; j < this.keys.Count; j++)
		{
			this.masterKey += this.keys[j];
		}
		DataManager.Save<MasterKeyData>(this.myMasterKeyData);
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x000504A0 File Offset: 0x0004E8A0
	private void prepWikis()
	{
		this.myWikiSiteData = DataManager.Load<WikiSiteData>(1919);
		if (this.myWikiSiteData == null)
		{
			this.myWikiSiteData = new WikiSiteData(1919);
			this.myWikiSiteData.Wikis = new List<WebSiteData>(this.wikis.Count);
			this.myWikiSiteData.WikiSites = new List<List<int>>(3);
			List<WebSiteDefinition> list = new List<WebSiteDefinition>(this.Websites);
			List<int> list2 = new List<int>(this.fakeDomains);
			for (int i = 0; i < this.wikis.Count; i++)
			{
				WebSiteData webSiteData = new WebSiteData();
				webSiteData.PageURL = MagicSlinger.MD5It(this.Websites[i].PageTitle + Time.time.ToString() + UnityEngine.Random.Range(0, 9999).ToString());
				this.myWikiSiteData.Wikis.Add(webSiteData);
			}
			for (int j = list.Count - 1; j >= 0; j--)
			{
				if (list[j].isFake)
				{
					list.RemoveAt(j);
				}
			}
			for (int k = 0; k < 3; k++)
			{
				Dictionary<int, string> dictionary = new Dictionary<int, string>(20);
				List<int> list3 = new List<int>(20);
				int num = 15;
				if (k != 0)
				{
					if (k != 1)
					{
						if (k == 2)
						{
							for (int l = list.Count - 1; l >= 0; l--)
							{
								if (list[l].WikiSpecific && list[l].WikiIndex == 2)
								{
									dictionary.Add(this.websiteLookUp[list[l].PageURL], list[l].PageTitle);
									list.RemoveAt(l);
									num--;
								}
							}
						}
					}
					else
					{
						for (int m = list.Count - 1; m >= 0; m--)
						{
							if (list[m].WikiSpecific && list[m].WikiIndex == 1)
							{
								dictionary.Add(this.websiteLookUp[list[m].PageURL], list[m].PageTitle);
								list.RemoveAt(m);
								num--;
							}
						}
					}
				}
				else
				{
					for (int n = list.Count - 1; n >= 0; n--)
					{
						if (list[n].WikiSpecific && list[n].WikiIndex == 0)
						{
							dictionary.Add(this.websiteLookUp[list[n].PageURL], list[n].PageTitle);
							list.RemoveAt(n);
							num--;
						}
					}
					bool flag = false;
					while (!flag)
					{
						int index = UnityEngine.Random.Range(0, list.Count);
						if (!list[index].WikiSpecific && !list[index].HasWindow)
						{
							list[index].HoldsSecondWikiLink = true;
							this.myWikiSiteData.PickedSiteToHoldSecondWiki = list[index].PageTitle.GetHashCode();
							dictionary.Add(this.websiteLookUp[list[index].PageURL], list[index].PageTitle);
							list.RemoveAt(index);
							num--;
							flag = true;
						}
					}
				}
				int num2 = 0;
				while (num2 < num)
				{
					int index2 = UnityEngine.Random.Range(0, list.Count);
					if (!list[index2].WikiSpecific)
					{
						dictionary.Add(this.websiteLookUp[list[index2].PageURL], list[index2].PageTitle);
						list.RemoveAt(index2);
						num2++;
					}
				}
				for (int num3 = 0; num3 < 10; num3++)
				{
					int index3 = UnityEngine.Random.Range(0, list2.Count);
					dictionary.Add(list2[index3], this.Websites[list2[index3]].PageTitle);
					list2.RemoveAt(index3);
				}
				List<KeyValuePair<int, string>> list4 = dictionary.ToList<KeyValuePair<int, string>>();
				list4.Sort((KeyValuePair<int, string> pair1, KeyValuePair<int, string> pair2) => pair1.Value.CompareTo(pair2.Value));
				for (int num4 = 0; num4 < list4.Count; num4++)
				{
					list3.Add(list4[num4].Key);
				}
				this.myWikiSiteData.WikiSites.Add(list3);
			}
		}
		this.wikiLookUp = new Dictionary<string, int>(this.myWikiSiteData.Wikis.Count);
		for (int num5 = 0; num5 < this.myWikiSiteData.Wikis.Count; num5++)
		{
			this.wikis[num5].PageURL = this.myWikiSiteData.Wikis[num5].PageURL;
			this.wikiLookUp.Add(this.myWikiSiteData.Wikis[num5].PageURL, num5);
		}
		for (int num6 = 0; num6 < this.myWikiSiteData.WikiSites.Count; num6++)
		{
			this.wikiSites.Add(this.myWikiSiteData.WikiSites[num6]);
		}
		for (int num7 = 0; num7 < this.Websites.Count; num7++)
		{
			if (this.Websites[num7].PageTitle.GetHashCode() == this.myWikiSiteData.PickedSiteToHoldSecondWiki)
			{
				this.Websites[num7].HoldsSecondWikiLink = true;
			}
			else
			{
				this.Websites[num7].HoldsSecondWikiLink = false;
			}
		}
		DataManager.Save<WikiSiteData>(this.myWikiSiteData);
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x00050AC8 File Offset: 0x0004EEC8
	private void prepWebsites()
	{
		this.myWebSitesData = DataManager.Load<WebSitesData>(2020);
		if (this.myWebSitesData == null)
		{
			this.myWebSitesData = new WebSitesData(2020);
			this.myWebSitesData.Sites = new List<WebSiteData>(this.Websites.Count);
			for (int i = 0; i < this.Websites.Count; i++)
			{
				WebSiteData webSiteData = new WebSiteData();
				webSiteData.Pages = new List<WebPageData>();
				if (!this.Websites[i].isStatic)
				{
					webSiteData.PageURL = MagicSlinger.MD5It(this.Websites[i].PageTitle + Time.time.ToString() + UnityEngine.Random.Range(0, 9999).ToString());
				}
				else
				{
					webSiteData.PageURL = this.Websites[i].PageURL;
				}
				webSiteData.Fake = this.Websites[i].isFake;
				webSiteData.Visted = false;
				webSiteData.IsTapped = false;
				WebPageData webPageData = new WebPageData();
				webPageData.KeyDiscoveryMode = UnityEngine.Random.Range(0, 4);
				webPageData.IsTapped = false;
				webPageData.HashIndex = 0;
				webPageData.HashValue = string.Empty;
				webSiteData.Pages.Add(webPageData);
				if (this.Websites[i].SubPages != null)
				{
					for (int j = 0; j < this.Websites[i].SubPages.Count; j++)
					{
						WebPageData webPageData2 = new WebPageData();
						webPageData2.KeyDiscoveryMode = UnityEngine.Random.Range(0, 4);
						webPageData2.IsTapped = false;
						webPageData2.HashIndex = 0;
						webPageData2.HashValue = string.Empty;
						webSiteData.Pages.Add(webPageData2);
					}
				}
				this.myWebSitesData.Sites.Add(webSiteData);
			}
			this.itsNewATap = true;
		}
		this.websiteLookUp = new Dictionary<string, int>(this.myWebSitesData.Sites.Count);
		for (int k = 0; k < this.myWebSitesData.Sites.Count; k++)
		{
			this.Websites[k].PageURL = this.myWebSitesData.Sites[k].PageURL;
			this.Websites[k].WasVisted = this.myWebSitesData.Sites[k].Visted;
			this.Websites[k].IsTapped = this.myWebSitesData.Sites[k].IsTapped;
			if (this.myWebSitesData.Sites[k].Fake)
			{
				this.fakeDomains.Add(k);
			}
			else if (this.myWebSitesData.Sites[k].Pages != null)
			{
				this.Websites[k].HomePage.KeyDiscoverMode = (KEY_DISCOVERY_MODES)this.myWebSitesData.Sites[k].Pages[0].KeyDiscoveryMode;
				this.Websites[k].HomePage.IsTapped = this.myWebSitesData.Sites[k].Pages[0].IsTapped;
				this.Websites[k].HomePage.HashIndex = this.myWebSitesData.Sites[k].Pages[0].HashIndex;
				this.Websites[k].HomePage.HashValue = this.myWebSitesData.Sites[k].Pages[0].HashValue;
				for (int l = 0; l < this.Websites[k].SubPages.Count; l++)
				{
					if (this.myWebSitesData.Sites[k].Pages[l + 1] != null)
					{
						this.Websites[k].SubPages[l].KeyDiscoverMode = (KEY_DISCOVERY_MODES)this.myWebSitesData.Sites[k].Pages[l + 1].KeyDiscoveryMode;
						this.Websites[k].SubPages[l].IsTapped = this.myWebSitesData.Sites[k].Pages[l + 1].IsTapped;
						this.Websites[k].SubPages[l].HashIndex = this.myWebSitesData.Sites[k].Pages[l + 1].HashIndex;
						this.Websites[k].SubPages[l].HashValue = this.myWebSitesData.Sites[k].Pages[l + 1].HashValue;
					}
				}
			}
			this.websiteLookUp.Add(this.myWebSitesData.Sites[k].PageURL, k);
		}
		DataManager.Save<WebSitesData>(this.myWebSitesData);
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x00051038 File Offset: 0x0004F438
	private void prepBookmarks()
	{
		this.myBookMarksData = DataManager.Load<BookMarksData>(2021);
		if (this.myBookMarksData == null)
		{
			this.myBookMarksData = new BookMarksData(2021);
			this.myBookMarksData.BookMarks = new Dictionary<int, BookmarkData>();
			BookmarkData value = new BookmarkData("Adam's Twitter", "http://www.twitter.com/thewebpro");
			BookmarkData value2 = new BookmarkData("Adam's YouTube", "http://www.youtube.com/c/ReflectStudios");
			this.myBookMarksData.BookMarks.Add(14, value);
			this.myBookMarksData.BookMarks.Add(15, value2);
		}
		this.bookmarks = new Dictionary<int, BookmarkData>(this.myBookMarksData.BookMarks.Count);
		foreach (KeyValuePair<int, BookmarkData> keyValuePair in this.myBookMarksData.BookMarks)
		{
			this.bookmarks.Add(keyValuePair.Key, keyValuePair.Value);
			GameManager.BehaviourManager.AnnBehaviour.AddBookmarkTab(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x00051168 File Offset: 0x0004F568
	private void tapSites()
	{
		if (this.itsNewATap)
		{
			List<string> list = new List<string>(this.keys);
			Dictionary<string, int> dictionary = new Dictionary<string, int>(8);
			for (int i = 0; i < this.keys.Count; i++)
			{
				dictionary.Add(this.keys[i], i);
			}
			for (int j = 0; j < this.wikiSites.Count; j++)
			{
				List<int> list2 = new List<int>(this.wikiSites[j]);
				int k = 0;
				while (k < 2)
				{
					int index = UnityEngine.Random.Range(0, list2.Count);
					WebSiteDefinition webSiteDefinition = this.Websites[list2[index]];
					if (!webSiteDefinition.isFake && !webSiteDefinition.DoNotTap && !webSiteDefinition.IsTapped)
					{
						webSiteDefinition.IsTapped = true;
						this.myWebSitesData.Sites[list2[index]].IsTapped = true;
						int index2 = UnityEngine.Random.Range(0, list.Count);
						string text = list[index2];
						int hashIndex = dictionary[text];
						if (webSiteDefinition.SubPages.Count > 0)
						{
							if (UnityEngine.Random.Range(0, 10) == 3)
							{
								webSiteDefinition.HomePage.IsTapped = true;
								webSiteDefinition.HomePage.HashIndex = hashIndex;
								webSiteDefinition.HomePage.HashValue = text;
								this.myWebSitesData.Sites[list2[index]].Pages[0].IsTapped = true;
								this.myWebSitesData.Sites[list2[index]].Pages[0].HashIndex = hashIndex;
								this.myWebSitesData.Sites[list2[index]].Pages[0].HashValue = text;
							}
							else
							{
								int num = UnityEngine.Random.Range(0, webSiteDefinition.SubPages.Count);
								webSiteDefinition.SubPages[num].IsTapped = true;
								webSiteDefinition.SubPages[num].HashIndex = hashIndex;
								webSiteDefinition.SubPages[num].HashValue = text;
								if (this.myWebSitesData.Sites[list2[index]].Pages[num + 1] != null)
								{
									this.myWebSitesData.Sites[list2[index]].Pages[num + 1].IsTapped = true;
									this.myWebSitesData.Sites[list2[index]].Pages[num + 1].HashIndex = hashIndex;
									this.myWebSitesData.Sites[list2[index]].Pages[num + 1].HashValue = text;
								}
							}
						}
						else
						{
							webSiteDefinition.HomePage.IsTapped = true;
							webSiteDefinition.HomePage.HashIndex = hashIndex;
							webSiteDefinition.HomePage.HashValue = text;
							this.myWebSitesData.Sites[list2[index]].Pages[0].IsTapped = true;
							this.myWebSitesData.Sites[list2[index]].Pages[0].HashIndex = hashIndex;
							this.myWebSitesData.Sites[list2[index]].Pages[0].HashValue = text;
						}
						list.RemoveAt(index2);
						dictionary.Remove(text);
						k++;
					}
					list2.RemoveAt(index);
				}
			}
			for (int l = 1; l < this.wikiSites.Count; l++)
			{
				List<int> list3 = new List<int>(this.wikiSites[l]);
				int k = 0;
				while (k < 1)
				{
					int index3 = UnityEngine.Random.Range(0, list3.Count);
					WebSiteDefinition webSiteDefinition2 = this.Websites[list3[index3]];
					if (!webSiteDefinition2.isFake && !webSiteDefinition2.DoNotTap && !webSiteDefinition2.IsTapped)
					{
						webSiteDefinition2.IsTapped = true;
						this.myWebSitesData.Sites[index3].IsTapped = true;
						int index4 = UnityEngine.Random.Range(0, list.Count);
						string text2 = list[index4];
						int hashIndex2 = dictionary[text2];
						if (webSiteDefinition2.SubPages.Count > 0)
						{
							if (UnityEngine.Random.Range(0, 10) == 3)
							{
								webSiteDefinition2.HomePage.IsTapped = true;
								webSiteDefinition2.HomePage.HashIndex = hashIndex2;
								webSiteDefinition2.HomePage.HashValue = text2;
								this.myWebSitesData.Sites[list3[index3]].Pages[0].IsTapped = true;
								this.myWebSitesData.Sites[list3[index3]].Pages[0].HashIndex = hashIndex2;
								this.myWebSitesData.Sites[list3[index3]].Pages[0].HashValue = text2;
							}
							else
							{
								int num2 = UnityEngine.Random.Range(0, webSiteDefinition2.SubPages.Count);
								webSiteDefinition2.SubPages[num2].IsTapped = true;
								webSiteDefinition2.SubPages[num2].HashIndex = hashIndex2;
								webSiteDefinition2.SubPages[num2].HashValue = text2;
								if (this.myWebSitesData.Sites[list3[index3]].Pages[num2 + 1] != null)
								{
									this.myWebSitesData.Sites[list3[index3]].Pages[num2 + 1].IsTapped = true;
									this.myWebSitesData.Sites[list3[index3]].Pages[num2 + 1].HashIndex = hashIndex2;
									this.myWebSitesData.Sites[list3[index3]].Pages[num2 + 1].HashValue = text2;
								}
							}
						}
						else
						{
							webSiteDefinition2.HomePage.IsTapped = true;
							webSiteDefinition2.HomePage.HashIndex = hashIndex2;
							webSiteDefinition2.HomePage.HashValue = text2;
							this.myWebSitesData.Sites[list3[index3]].Pages[0].IsTapped = true;
							this.myWebSitesData.Sites[list3[index3]].Pages[0].HashIndex = hashIndex2;
							this.myWebSitesData.Sites[list3[index3]].Pages[0].HashValue = text2;
						}
						list.RemoveAt(index4);
						dictionary.Remove(text2);
						k++;
					}
				}
			}
			DataManager.Save<WebSitesData>(this.myWebSitesData);
		}
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x000518AB File Offset: 0x0004FCAB
	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.prepWebsites();
		this.prepWikis();
		this.prepBookmarks();
		this.prepTheMasterKey();
		this.tapSites();
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x000518E4 File Offset: 0x0004FCE4
	private void Awake()
	{
		GameManager.TheCloud = this;
		this.validDomains.Add("youtube.com");
		this.validDomains.Add("twitch.tv");
		this.validDomains.Add("twitter.com");
		GameManager.StageManager.Stage += this.stageMe;
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x0005193D File Offset: 0x0004FD3D
	private void Update()
	{
		if (this.rollCoolDownActive && Time.time - this.rollTimeStamp >= 30f)
		{
			this.rollCoolDownActive = false;
		}
	}

	// Token: 0x04000EDD RID: 3805
	public CustomEvent KeyDiscoveredEvent = new CustomEvent(6);

	// Token: 0x04000EDE RID: 3806
	private const string NOT_FOUND_URL = "localGame://NotFound/index.html";

	// Token: 0x04000EDF RID: 3807
	private const string DOC_ROOT = "localGame://";

	// Token: 0x04000EE0 RID: 3808
	[SerializeField]
	private List<WebSiteDefinition> wikis;

	// Token: 0x04000EE1 RID: 3809
	[SerializeField]
	private List<WebSiteDefinition> Websites;

	// Token: 0x04000EE2 RID: 3810
	private List<string> validDomains = new List<string>();

	// Token: 0x04000EE3 RID: 3811
	private List<int> fakeDomains = new List<int>();

	// Token: 0x04000EE4 RID: 3812
	private List<List<int>> wikiSites = new List<List<int>>();

	// Token: 0x04000EE5 RID: 3813
	private List<string> keys = new List<string>();

	// Token: 0x04000EE6 RID: 3814
	private Dictionary<string, int> wikiLookUp = new Dictionary<string, int>();

	// Token: 0x04000EE7 RID: 3815
	private Dictionary<string, int> websiteLookUp = new Dictionary<string, int>();

	// Token: 0x04000EE8 RID: 3816
	private Dictionary<int, BookmarkData> bookmarks = new Dictionary<int, BookmarkData>();

	// Token: 0x04000EE9 RID: 3817
	private Dictionary<string, string> passwordList = new Dictionary<string, string>();

	// Token: 0x04000EEA RID: 3818
	private WebSiteDefinition curWebsiteDef;

	// Token: 0x04000EEB RID: 3819
	private WebSiteDefinition lastSourceWebSiteDef;

	// Token: 0x04000EEC RID: 3820
	private WebPageDefinition curWebPageDef;

	// Token: 0x04000EED RID: 3821
	private WebPageDefinition lastSourceWebPageDef;

	// Token: 0x04000EEE RID: 3822
	private WikiSiteData myWikiSiteData;

	// Token: 0x04000EEF RID: 3823
	private WebSitesData myWebSitesData;

	// Token: 0x04000EF0 RID: 3824
	private BookMarksData myBookMarksData;

	// Token: 0x04000EF1 RID: 3825
	private MasterKeyData myMasterKeyData;

	// Token: 0x04000EF2 RID: 3826
	private bool itsNewATap;

	// Token: 0x04000EF3 RID: 3827
	private string masterKey;

	// Token: 0x04000EF4 RID: 3828
	private bool rollCoolDownActive;

	// Token: 0x04000EF5 RID: 3829
	private float rollTimeStamp;
}
