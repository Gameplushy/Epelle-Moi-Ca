using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using KeepCoding;

public class EpelleMoiCaScript : ModuleScript {

    public KMSelectable[] keyboard;
    public GameObject[] letters;

    public KMSelectable play;
    public KMSelectable red;
    public KMSelectable green;
    public TextMesh word;
    public TextMesh timer;

    int chosenWord = 0;
    bool typing = false;
    private readonly string[][] wordList = new string[][]
    {
        new string[] {"accueil","accueils"},
        new string[] {"abasourdi","abasourdie","abasourdis","abasourdies"},
        new string[] {"aberrant","aberrants"},
        new string[] {"abrasive","abrasives"},
        new string[] {"acatalectique","acatalectiques"},
        new string[] {"acrobatie","acrobaties"},
        new string[] {"aligot","aligots"},
        new string[] {"amphigourique","amphigouriques"},
        new string[] {"analgésiante","analgésiantes"},
        new string[] {"antipasti","antipastis"},
        new string[] {"apparition","apparitions"},
        new string[] {"approvisionnement","approvisionnements"},
        new string[] {"aptonyme","aptonymes"},
        new string[] {"axolotl","axolotls"},
        new string[] {"aïeux"},
        new string[] {"baril","barils"},
        new string[] {"bedonnant","bedonnants"},
        new string[] {"berlingot","berlingots"},
        new string[] {"burlesque","burlesques"},
        new string[] {"canneberge","canneberges"},
        new string[] {"capharnaüm","capharnaüms"},
        new string[] {"caravansérail","caravansérails"},
        new string[] {"cassis"},
        new string[] {"cathéter","cathéters"},
        new string[] {"chiffonnade","chiffonnades"},
        new string[] {"chrysanthème","chrysanthèmes"},
        new string[] {"coccyx"},
        new string[] {"croquignolesque","croquignolesques"},
        new string[] {"curcubitacé","curcubitacés","curcubitacée","curcubitacées","cucurbitaceae"},
        new string[] {"dichotomie","dichotomies"},
        new string[] {"dictionnaire","dictionnaires"},
        new string[] {"ecchymose","ecchymoses"},
        new string[] {"effiloche","effiloches","effilochent"},
        new string[] {"équilateral","équilaterale","équilaterales","équilatéral","équilatérale","équilatérales"},
        new string[] {"équinoxe","équinoxes"},
        new string[] {"éraflure","éraflures"},
        new string[] {"gourgandine","gourgandines"},
        new string[] {"gringalet","gringalets"},
        new string[] {"hallogène","hallogènes"},
        new string[] {"histrion","histrions"},
        new string[] {"hortillonnage","hortillonnages"},
        new string[] {"hurluberlu","hurluberlus"},
        new string[] {"hypothétique","hypothétiques"},
        new string[] {"hébéphrénie","hébéphrénies"},
        new string[] {"idylle","idylles"},
        new string[] {"incarcération","incarcérations"},
        new string[] {"inexorable","inexorables"},
        new string[] {"iridescent","iridescents"},
        new string[] {"kamoulox"},
        new string[] {"kérosène","kérosènes"},
        new string[] {"lagéniforme","lagéniformes"},
        new string[] {"luthérien","luthériens"},
        new string[] {"maelström","maelströms","maëlstrom","maëlstroms","maelstrom","maelstroms"},
        new string[] {"mirliflore","mirliflores"},
        new string[] {"myxomatose","myxomatoses"},
        new string[] {"méningocoque","méningocoques"},
        new string[] {"neurasthénique","neurasthéniques"},
        new string[] {"nihilisme","nihilismes"},
        new string[] {"nonobstant","nonobstants"},
        new string[] {"œuvre", "œuvres"},
        new string[] {"omniprésence","omniprésences"},
        new string[] {"oxymore","oxymores"},
        new string[] {"paillette","paillettes"},
        new string[] {"palefrenière","palefrenières"},
        new string[] {"personnalité","personnalités"},
        new string[] {"plexiglas"},
        new string[] {"pluviôse","pluviôses"},
        new string[] {"polymnie","polymnies"},
        new string[] {"polysyndète","polysyndètes"},
        new string[] {"popeline","popelines"},
        new string[] {"presbytère","presbytères"},
        new string[] {"profession","professions"},
        new string[] {"pâmoison","pâmoisons"},
        new string[] {"péjorative","péjoratives"},
        new string[] {"péroné","péronés"},
        new string[] {"pérégrination","pérégrinations"},
        new string[] {"quadripède","quadripèdes"},
        new string[] {"quenouillon","quenouillons"},
        new string[] {"quetsche","quetsches"},
        new string[] {"quolibet","quolibets"},
        new string[] {"remembrance","remembrances"},
        new string[] {"rhododendron","rhododendrons"},
        new string[] {"serpillère","serpillères"},
        new string[] {"spleen","spleens"},
        new string[] {"sycophante","sycophantes"},
        new string[] {"syllepse","syllepses"},
        new string[] {"syllogisme","syllogismes"},
        new string[] {"terminus"},
        new string[] {"thyroïde","thyroïdes"},
        new string[] {"thébaïde","thébaïdes"},
        new string[] {"torréfaction","torréfactions"},
        new string[] {"ubiquité","ubiquités"},
        new string[] {"vagabond","vagabonds"},
        new string[] {"vertèbre","vertèbres"},
        new string[] {"victuaille","victuailles"},
        new string[] {"virevolte","virevoltes","virevoltent"},
        new string[] {"véracité","véracités"},
        new string[] {"ytterbium"},
        new string[] {"zeugma","zeugmas"},
        new string[] {"ziggurat","ziggurats","ziggourat","ziggourats"}
    };
    private readonly Dictionary<char, Dictionary<char, char>> accentDictionnary = new Dictionary<char, Dictionary<char, char>>()
    {
        {'G', new Dictionary<char, char>(){ { 'A', 'À' },{'E', 'È'},{ 'U', 'Ù' } } },
        {'C', new Dictionary<char, char>(){ { 'A', 'Â' },{'E', 'Ê' },{ 'I', 'Î' }, { 'O', 'Ô' }, { 'U', 'Û' } } },
        {'U', new Dictionary<char, char>(){ { 'A', 'Ä' },{'E', 'Ë' },{ 'I', 'Ï' }, { 'O', 'Ö' }, { 'U', 'Ü' } } },
    };
    string alphabet = "AZERTYUIOPQSDFGHJKLMWXCVBNÇŒÉ";
    string currentText = "";
    char accentMode = 'N'; //N one, G rave, C ircumflex, U mlaut
#pragma warning disable IDE0052 // Supprimer les membres privés non lus
    string inputtedText = "";
#pragma warning restore IDE0052 // Supprimer les membres privés non lus

    Coroutine timerCo;

    void Start () {
        keyboard.Assign(onInteract: KeyPress);
        play.OnInteract += delegate () { PlayButton(); return false; };
        green.OnInteract += delegate () { GreenButton(); return false; };
        red.OnInteract += delegate () { RedButton(); return false; };
		chosenWord = UnityEngine.Random.Range(0, wordList.Length);
    }

    void KeyPress(int pressedKey)
    {
        PlaySound(KMSoundOverride.SoundEffect.ButtonPress);
        if (typing)
        {
            if (accentMode == 'N')
            {
                if (pressedKey < alphabet.Length)
                {
                    currentText += alphabet[pressedKey];
                    word.text = currentText;
                }
                else
                {
                    accentMode = "GCU"[pressedKey - 29];
                    ChangeAccentMode();
                }
            }
            else
            {
                if (letters[pressedKey].activeSelf)
                {
                    if (pressedKey < 29)
                    {
                        char c = letters[pressedKey].GetComponent<TextMesh>().text[0];
                        c = accentDictionnary[accentMode][c];
                        currentText += c;
                        word.text = currentText;
                    }
                    accentMode = 'N';
                    ChangeAccentMode();
                }
            }
        }
    }

    private void ChangeAccentMode()
    {
        if (accentMode == 'N')
            letters.ForEach(l => l.SetActive(true));
        else
        {
            int[] displayThese;
            switch (accentMode)
            {
                case 'G':
                    displayThese = new int[] { 0, 2, 6, 29 };
                    break;
                case 'C':
                    displayThese = new int[] { 0, 2, 6, 7, 8, 30 };
                    break;
                case 'U':
                    displayThese = new int[] { 0, 2, 6, 7, 8, 31 };
                    break;

                default:
                    throw new ArgumentException("Something's wrong [{0}]. Please contact Konoko in order to fix this.",accentMode.ToString());
            }
            for(int i = 0; i < letters.Length; i++)
                letters[i].SetActive(displayThese.Contains(i));
        }
    }

    void PlayButton()
    {
        if (!IsSolved)
        {
            ButtonEffect(play, 1, wordList[chosenWord][0]);
            if (!typing)
            {
                typing = true;
                currentText = "";
                word.text = currentText;
                Log("Your word is: '{0}'.{1}", wordList[chosenWord][0], wordList[chosenWord].Length != 1 ? " {0} {1} also valid answers.".Form(wordList[chosenWord].Skip(1).Select(a=>"'"+a+"'"), wordList[chosenWord].Length == 2 ? "is" : "are") : "");
                timerCo = StartCoroutine(Ticking());
            }
        }
    }

    private IEnumerator Ticking()
    {
        int seconds = 60;
        while (seconds > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            seconds--;
            timer.text = seconds.ToString();
        }
        Log("You took to long to spell your word, module striked and reset.");
        Strike();
        Reset();
    }

    void GreenButton()
    {
        if(timerCo!=null) StopCoroutine(timerCo);
        ButtonEffect(green, 1, KMSoundOverride.SoundEffect.ButtonPress);
        if (typing)
        {
            Log("You spelt it: '{0}'",  currentText);
            if (wordList[chosenWord].Contains(currentText.ToLower()))
            {
                inputtedText = currentText.ToLower();
                Log("You spelled it correctly, module solved.");
                if (wordList[chosenWord][0].Equals("kamoulox")) PlaySound("specialEnding");
                else PlaySound(KMSoundOverride.SoundEffect.CorrectChime);
                Solve();
                ResetFrontEnd();
                ResetBackEnd();
            }
            else
            {
                Log("You spelled it incorrectly, module striked and reset.");
                Strike();
                Reset();
            }
        }
    }

    private void Reset()
    {
        chosenWord = UnityEngine.Random.Range(0, wordList.Length);
        ResetFrontEnd();
        ResetBackEnd();
    }

    private void ResetFrontEnd()
    {
        timer.text = "60";
        word.text = "ÉPELLE-MOI ÇA";
    }

    private void ResetBackEnd()
    {
        timerCo = null;
        typing = false;
    }

    void RedButton()
    {
        red.AddInteractionPunch();
        PlaySound(KMSoundOverride.SoundEffect.ButtonPress);
        if (typing)
        {
            currentText = "";
            word.text = currentText;
        }
    }

    private bool IsValidWord(string s)
    {
        List<string> valids = Enumerable.Range('A', 26).Select(i => ((char)i).ToString()).ToList();
        valids.AddRange("ÇŒÉÀÈÙÂÊÎÔÛÄËÏÖÜ".ToList().Select(a=>a.ToString()));
        valids.AddRange(new List<string> { "(CC)", "(OE)", "(AE)", "(GA)", "(GE)", "(GU)", "(CA)", "(CE)", "(CI)", "(CO)", "(CU)", "(UA)", "(UE)", "(UI)", "(UO)", "(UU)" });

        int index = 0;
        while (index < s.Length)
        {
            string checkThis;
            if (s[index].Equals('('))
            {
                if (index + 3 >= s.Length || !s[index + 3].Equals(')')) return false;
                checkThis = s.Substring(index, 4);
                index += 4;
            }
            else
            {
                checkThis = s[index].ToString();
                index++;
            }
            if (!valids.Contains(checkThis)) return false;
        }
        return true;
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"[!{0} play #] Presses the play button # times (# is optional). [!{0} submit <word>] Submits the specified word. Accented letter can be sent as it, or you can send (CC) instead of Ç, (OE) for Œ, (AE) for É, (GX) for grave X, (CX) for circumflex X and (UX) for umlaut X, X being a vowel (AEU only for graves). Submitted words must be in all caps, no spaces even between parantheses.";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.Trim();
        if (Regex.IsMatch(command, @"^play(\s[0-9]*)?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            string[] commands = command.Split(' ');
            int repeats = commands.Length == 1 ? 1 : int.Parse(commands.Last());
            if (repeats == 0) yield break;

            yield return null;
            for(int i = 0; i < repeats; i++)
            {
                play.OnInteract();
                if(i<repeats-1) yield return new WaitForSeconds(2f);
            }
            yield break;
        }
        
        string[] parameters = command.Split(' ');
        if (parameters.Length==2 && parameters[0].Equals("Submit",StringComparison.InvariantCultureIgnoreCase) && IsValidWord(parameters[1]))
        {
            yield return null;
            List<int> buttonPresses = GetCommandList(parameters[1]);
            foreach(int index in buttonPresses)
            {
                keyboard[index].OnInteract();
                yield return new WaitForSeconds(.18f);
            }
            green.OnInteract();
        }
    }

    private List<int> GetCommandList(string word)
    {
        string basicLetters = "ÇŒÉ";
        string complexLetters = "ÀÈÙÂÊÎÔÛÄËÏÖÜ";
        string normalLetters =  "AEUAEIOUAEIOU";
        string[] basicLetterCodes = { "(CC)", "(OE)", "(AE)" };
        string[] complexLetterCodes = { "(GA)", "(GE)", "(GU)", "(CA)", "(CE)", "(CI)", "(CO)", "(CU)", "(UA)", "(UE)", "(UI)", "(UO)", "(UU)" };
        List<int> buttonIndexes = new List<int>();
        int index = 0;
        while (index < word.Length)
        {
            string checkThis;
            if (word[index].Equals('('))
            {
                checkThis = word.Substring(index, 4);
                index += 4;
            }
            else
            {
                checkThis = word[index].ToString();
                index++;
            }
            if (alphabet.Contains(checkThis)) buttonIndexes.Add(alphabet.IndexOf(checkThis));
            else if (basicLetters.Contains(checkThis)) buttonIndexes.Add(basicLetters.IndexOf(checkThis) + 26);
            else if (basicLetterCodes.Contains(checkThis)) buttonIndexes.Add(basicLetterCodes.IndexOf(checkThis) + 26);
            else
            {
                int i;
                if (complexLetters.Contains(checkThis)) i = complexLetters.IndexOf(checkThis);
                else i = complexLetterCodes.IndexOf(checkThis);
                if (i <= 2) buttonIndexes.Add(29);
                else if (i >= 8) buttonIndexes.Add(31);
                else buttonIndexes.Add(30);
                buttonIndexes.Add(alphabet.IndexOf(normalLetters[i]));
            }
        }
        return buttonIndexes;
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        if (!typing)
        {
            yield return ProcessTwitchCommand("play");
            yield return new WaitForSecondsRealtime(1f);
        }
        yield return ProcessTwitchCommand("submit " + wordList[chosenWord][0].ToUpper());
    }
}
