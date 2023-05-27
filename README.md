# kajarendeloapp
kaja rendelgetős applikációcska

User class 
↓↓↓↓↓↓↓↓↓
user tulajdonságai/függvényei: 
- currUser: eltárolja a jelenleg bejelentkezett felhasználót
- userList: ebben van az összes fiók
- public ID: egy szám ami azt tárolja, hogy hanyadjára regisztrálták a fiókot
- public UserName: szerintem ez magától értetődő
- private Password: ez is
- public Email: és ez is
- unKey: titkosítási kulcs a felhasználónévhez
- pwKey: titkosítási kulcs a jelszóhoz
- emKey: titkosítási kulcs az email címhez
- Perms: a felhasználó jogosultságai ("a", ha admin, "n", ha sima felhasználó, a 0. user pedig mindenki felett áll)
- chars: előfordulható karakterek
- encrTable: egy bővített Vigenere cipher


- public User konstruktor: regisztrál egy fiókot, ha érvényesek a megadott adatok, és titkosítja az adatokat
- private User konstruktor: beolvas egy fiókot
- public static void GetUsers: beolvassa a fájlból a usereket és berakja a userList-be őket, ha nincs fájl, akkor pedig csinál egyet és generál egy admin fiókot (!!! fontos, ezt az initializecomponent után muszáj odaírni)
- public static void OnExit: kilépéskor hiddeneli a fájlokat, ezt implementálni kell a mainwindow fájlban 
  ↓↓↓↓↓↓↓↓↓
  protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
  {
     User.OnExit();
     Environment.Exit(0);
  }
  
- public static void Login: megnézi, hogy szerepel-e a fiókok közt a megadott felhasználónév és jelszó, majd ha igen, elmenti melyik
- public static void LogOut: kilép a fiókból
- private static bool DeleteUser: eltávolít egy fiókot és visszaad egy bool-t, hogy sikeres volt-e
- private static void EncryptString: titkosít egy stringet
- private static void DecryptString: visszafejt egy stringet
- private static bool isEmailAddress: megnézi, hogy a megadott email cím valid-e
- public static void EditUser: jogosultság alapján megnyit egy másik ablakot, amiben módosítani lehet az adatokat

InputBox class
↓↓↓↓↓↓↓↓↓
netről szedtem és átírtam, megjelenít egy ablakot, ahol meg lehet adni egy jelszót

GridBox class
↓↓↓↓↓↓↓↓↓
ezt már én csináltam teljesen, a grid az alap usereknek négy oszlopos és csak saját magukat látják, az adminoknak 6 és látnak/tudnak módosítani minden alap usert
- konstruktor: legenerál egy ablakot egy griddel
- grid_Loaded_0 és grid_Loaded_1: jogosultság alapján generálja le a gridet (az eredeti account mindent módosíthat, a sima admin minden alap usert és magát, az alap user pedig csak magát)
- button_Clicked: módosít egy jelszót
- button2_Clicked: töröl egy fiókot
- ComboBoxSelectionChanged: csak a 0. usernek van, ő állíthatja be minden másik user jogosultságát
- tb_TextChanged: ha a 0. usert akarják módosítani másik adminok, nem engedi
- tb1_GotFocus és tb2_GotFocus: ha a 0. usert akarák módosítani hibát dob, ha nem, akkor elmenti az eredeti stringet
- tb1_LostFocus: ha valid az új felhasználónév, akkor generál hozzá egy új kulcsot és elmenti
- tb2_LostFocus: ha valid az új email cím, akkor generál hozzá egy új kulcsot és elmenti
