# wttg2-website-unblocker
This is a mod/patch for the game "Welcome to the Game 2" that unblocks all websites on the in-game web-browser.

![image](https://github.com/user-attachments/assets/a7ec9724-2816-40ce-9cbe-0f90a4735876)

![image](https://github.com/user-attachments/assets/3ea8095a-ec83-4629-88f9-36e4712914e2)

---

## How to: 
- Go to `C:\Program Files (x86)\Steam\steamapps\common\Welcome to the Game II\WTTG2_Data\Managed`
- Rename `Assembly-CSharp.dll` to something like `Assembly-CSharp.dll-BAK`
- Download the [Assembly-CSharp.dll](https://github.com/NullDev/wttg2-website-unblocker/raw/refs/heads/master/Assembly-CSharp.dll) from this repo.
- Put it where you renamed the other `Assembly-CSharp.dll` (still `C:\Program Files (x86)\Steam\steamapps\common\Welcome to the Game II\WTTG2_Data\Managed`)
- Done.
- Yay.

---

## Or patch it yourself. (only if you know what you're doing)
- Use [DnSpy](https://github.com/dnSpy/dnSpy) to read `C:\Program Files (x86)\Steam\steamapps\common\Welcome to the Game II\WTTG2_Data\Managed\Assembly-CSharp.dll` (Note: [ILSpy](https://github.com/icsharpcode/ILSpy) did _not_ work)
- Apply the [exact patch](https://github.com/NullDev/wttg2-website-unblocker/blob/master/diff.patch) from the repo to `Assembly-CSharp.dll` -> `_` -> `TheCloud @02000242`

  ![image](https://github.com/user-attachments/assets/ddaf3450-c6cd-4886-948e-e4655f42a12b)

- compile (leave all fields default. no not keep signatures)
- done.
- yay.

---

## Todo

Maybe ill write an injector or somethin
