using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace xddddd
{
    //felhasználó class
    public class User
    {
        //jelenleg belépett user
        public static User currUser { get; private protected set; } = null;
        //lista, ami tartalmazza az összes usert
        private protected static List<User> userList { get; set; } = new List<User>();
        //random szám generátor obv
        private protected static Random rnd = new Random();
        //jelszó titkosító kulcs property
        private protected string pwkey;
        private protected string pwKey { get => pwkey; set => pwkey = value; }
        //felhasználónév titkosító kulcs property
        private protected string unkey;
        private protected string unKey { get => unkey; set => unkey = value; }
        //email titkosító kulcs property
        private protected string emkey;
        private protected string emKey { get => emkey; set => emkey = value; }
        //felhasználónév property
        private protected string userName;
        public string UserName { get => userName; set => userName = value; }
        //titkosított jelszó property
        private protected string PW;
        private protected string Password { get => PW; set => PW = value; }
        //ID property
        private protected string id;
        public string ID { get => id; private protected set => id = value; }
        //jogok (admin: "a", alap: "n"), muszáj megadni
        private protected string perms;
        public string Perms { get => perms; private protected set => perms = value; }
        //email cím
        private protected string email;
        public string Email { get => email; set => email = value; }
        //előforduló karakterek, hosszú
        private protected static readonly char[] chars = new char[62]
        {
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','0','1','2','3','4','5','6','7','8','9'
        };
        //az encryption table, igen, ez még hosszabb  
        private protected static readonly char[,] encrTable = new char[62, 62]
        {
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, { 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a' }, { 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b' }, { 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c' }, { 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd' }, { 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e' }, { 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' }, { 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g' }, { 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' }, { 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i' }, { 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' }, { 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k' }, { 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l' }, { 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm' }, { 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n' }, { 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o' }, { 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' }, { 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q' }, { 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r' }, { 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's' }, { 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't' }, { 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u' }, { 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v' }, { 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w' }, { 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x' }, { 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y' }, { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' }, { 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A' }, { 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B' }, { 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C' }, { 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D' }, { 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E' }, { 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F' }, { 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G' }, { 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' }, { 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' }, { 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }, { 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K' }, { 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L' }, { 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' }, { 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N' }, { 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' }, { 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P' }, { 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q' }, { 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R' }, { 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S' }, { 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T' }, { 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U' }, { 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V' }, { 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W' }, { 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X' }, { 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y' }, { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }, { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0' }, { '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1' }, { '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2' }, { '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3' }, { '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4' }, { '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5' }, { '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6' }, { '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7' }, { '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8' }
        };
        //felhasználó hozzáadó konstruktor (regisztráció)
        public User(string username, string password, string email, string perms = "n")
        {
            File.SetAttributes("userData.txt", FileAttributes.Normal);
            File.SetAttributes("keys.txt", FileAttributes.Normal);
            File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
            try
            {
                if (username != string.Empty && password != string.Empty)
                {
                    //megnézi, hogy csak megfelelő karakterek szerepelnek-e benne
                    if (username.Length < 6 || password.Length < 6 || username.Length > 32 || password.Length > 32)
                    {
                        throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                    }
                    foreach (char c in username)
                    {
                        if (!chars.Contains(c))
                        {
                            throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                        }
                    }
                    foreach (char c in password)
                    {
                        if (!chars.Contains(c))
                        {
                            throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                        }
                    }
                    if (currUser != null && currUser.ID == "0")
                    {
                        if (MessageBox.Show("Admint akarsz hozzáadni?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            perms = "a";
                        }
                    }                    
                    string[] x = File.ReadAllLines("userData.txt");
                    string[] y = File.ReadAllLines("keys.txt");
                    for (int i = 0; i < x.Count(); i++)
                    {
                        if (DecryptString(x[i].Split(';')[1], y[i].Split(';')[1]) == username)
                        {
                            throw new Exception("Már van ilyen nevű fiók!");
                        }
                    }
                    if (username == password)
                    {
                        throw new Exception("A felhasználónév és a jelszó nem egyezhet meg!");
                    }
                    //generál egy random, a felhasználónév hosszával megegyező titkosítási kulcsot
                    int len = 0;
                    while (len < username.Length)
                    {
                        unkey += chars[rnd.Next(0, 61)].ToString();
                        len++;
                    }
                    //generál egy random, a jelszó hosszával megegyező titkosítási kulcsot
                    len = 0;
                    while (len < password.Length)
                    {
                        pwkey += chars[rnd.Next(0, 61)].ToString();
                        len++;
                    }
                    userName = username;
                    PW = password;
                    id = File.ReadAllLines("userData.txt").Length.ToString();
                    this.perms = perms;
                    //megnézi, hogy valid-e az email cím és generál hozzá egy random titkosítási kulcsot, ha meg van adva
                    if (email == string.Empty)
                    {
                        throw new Exception("Adj meg email címet!");
                    }
                    else
                    {
                        try
                        {
                            if (isEmailAddress(email))
                            {
                                string[] n = email.Split('@', '.');
                                for (int i = 0; i < n.Length; i++)
                                {
                                    len = 0;
                                    while (len < n[i].Length)
                                    {
                                        emkey += chars[rnd.Next(0, 61)].ToString();
                                        len++;
                                    }
                                    switch (i)
                                    {
                                        case 0:
                                            emkey += "@";
                                            break;
                                        case 1:
                                            emkey += ".";
                                            break;
                                    }
                                }
                                this.email = email;
                            }
                        }
                        catch
                        {

                            throw new Exception("Nem megfelelő email cím formátum.\\n\\nHelyes formátum: x@y.z");
                        }
                    }
                    //hozzáadja a user listához
                    userList.Add(this);
                    //beírja az új elemet a txt fájlba                 
                    if (ID == "0" || File.ReadAllLines("userData.txt")[File.ReadAllLines("userData.txt").Length - 1] == "Deleted;")
                    {
                        File.AppendAllText("userData.txt", ID + ";" + EncryptString(UserName, unKey) + ";" + EncryptString(Password, pwKey) + ";" + EncryptString(Email.Split('@', '.')[0], emKey.Split('@', '.')[0]) + "@" + EncryptString(Email.Split('@', '.')[1], emKey.Split('@', '.')[1]) + "." + EncryptString(Email.Split('@', '.')[2], emKey.Split('@', '.')[2]) + ";");
                        File.AppendAllText("keys.txt", ID + ";" + unKey + ";" + pwKey + ";" + emKey + ";" + Perms + ";");
                    }
                    else
                    {
                        File.AppendAllText("userData.txt", "\r\n" + ID + ";" + EncryptString(UserName, unKey) + ";" + EncryptString(Password, pwKey) + ";" + EncryptString(Email.Split('@', '.')[0], emKey.Split('@', '.')[0]) + "@" + EncryptString(Email.Split('@', '.')[1], emKey.Split('@', '.')[1]) + "." + EncryptString(Email.Split('@', '.')[2], emKey.Split('@', '.')[2]) + ";");
                        File.AppendAllText("keys.txt", "\r\n" + ID + ";" + unKey + ";" + pwKey + ";" + emKey + ";" + Perms + ";");
                        Login(UserName, Password, false);
                        MessageBox.Show("Sikeres regisztráció!\nBeléptél ezzel a fiókkal: " + currUser.UserName);
                    }
                }
                else
                {
                    throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                }
            }
            catch (Exception e)
            {
                File.SetAttributes("userData.txt", FileAttributes.Hidden);
                File.SetAttributes("keys.txt", FileAttributes.Hidden);
                File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                id = string.Empty;
                userName = string.Empty;
                PW = string.Empty;
                this.email = string.Empty;
                pwkey = string.Empty;
                unKey = string.Empty;
                this.perms = string.Empty;
                MessageBox.Show(e.Message);
            }
            File.SetAttributes("userData.txt", FileAttributes.Hidden);
            File.SetAttributes("keys.txt", FileAttributes.Hidden);
            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
        }
        //felhasználó beolvasó konstruktor (csak a beolvasó methodnek)
        private protected User(string id, string username, string encrPw, string email, string unkey, string pwkey, string emkey, string perms)
        {
            this.id = id;
            userName = DecryptString(username, unkey);
            PW = DecryptString(encrPw, pwkey);
            this.email = DecryptString(email.Split('@', '.')[0], emkey.Split('@', '.')[0], 0) + "@" + DecryptString(email.Split('@', '.')[1], emkey.Split('@', '.')[1], 1) + "." + DecryptString(email.Split('@', '.')[2], emkey.Split('@', '.')[2], 2);
            this.unkey = unkey;
            this.pwkey = pwkey;
            this.emkey = emkey;
            this.perms = perms;
        }
        //!!!felhasználó beolvasó method, rögtön az InitializeComponent() után megy, utána ne használd
        public static void GetUsers()
        {
            try
            {
                if (File.Exists("userData.txt") && File.Exists("keys.txt") && File.Exists("currentLogin.txt"))
                {
                    File.SetAttributes("userData.txt", FileAttributes.Normal);
                    File.SetAttributes("keys.txt", FileAttributes.Normal);
                    File.SetAttributes("currentLogin.txt", FileAttributes.Normal);                   
                    string[] x = File.ReadAllLines("userData.txt");
                    string[] y = File.ReadAllLines("keys.txt");
                    string[] z = File.ReadAllLines("currentLogin.txt");
                    if (!x.Length.Equals(y.Length))
                    {
                        throw new Exception("Hiba történt.");
                    }
                    else if (x.Length > 0 && y.Length > 0)
                    {
                        userList.Clear();
                        for (int i = 0; i < x.Length; i++)
                        {
                            if (x[i] != "Deleted;")
                            {
                                userList.Add(new User(x[i].Split(';')[0], x[i].Split(';')[1], x[i].Split(';')[2], x[i].Split(';')[3], y[i].Split(';')[1], y[i].Split(';')[2], y[i].Split(';')[3], y[i].Split(';')[4]));
                            }
                        }
                    }
                    if (z.Length != 0)
                    {
                        currUser = new User(z[0].Split(';')[0], z[0].Split(';')[1], z[0].Split(';')[2], z[0].Split(';')[3], z[0].Split(';')[4], z[0].Split(';')[5], z[0].Split(';')[6], z[0].Split(';')[7]);
                    }
                }
                else
                {
                    FileStream f1 = File.Open("userData.txt", FileMode.OpenOrCreate);
                    File.SetAttributes(f1.Name, FileAttributes.Hidden);
                    FileStream f2 = File.Open("keys.txt", FileMode.OpenOrCreate);
                    File.SetAttributes(f2.Name, FileAttributes.Hidden);                    
                    FileStream f3 = File.Open("currentLogin.txt", FileMode.OpenOrCreate);
                    File.SetAttributes(f3.Name, FileAttributes.Hidden);
                    f1.Close();
                    f2.Close();
                    f3.Close();
                    new User("admin1", "admin2", "admin@admin.com", "a");
                }
            }
            catch (Exception e)
            {
                File.SetAttributes("userData.txt", FileAttributes.Hidden);
                File.SetAttributes("keys.txt", FileAttributes.Hidden);
                File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                MessageBox.Show(e.Message);
            }
            File.SetAttributes("userData.txt", FileAttributes.Hidden);
            File.SetAttributes("keys.txt", FileAttributes.Hidden);
            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
        }
        //mikor bezárja a programot, visszarejti a fájlokat
        public static void OnExit()
        {
            File.SetAttributes("userData.txt", FileAttributes.Hidden);
            File.SetAttributes("keys.txt", FileAttributes.Hidden);
            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
        }
        //megnézi, hogy szerepel-e a fiókok közt a megadott felhasználónév és jelszó, majd ha igen, elmenti melyik
        //a messageboxhoz ne nyúlj, az mindig true, kivéve mikor regisztrál és automatikusan bejelentkezik
        public static void Login(string username, string password, bool doLoginPopup = true)
        {
            File.SetAttributes("userData.txt", FileAttributes.Normal);
            File.SetAttributes("keys.txt", FileAttributes.Normal);
            File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
            try
            {
                bool isLoginSuccessful = false;
                if (currUser != null && username == currUser.UserName)
                {
                    throw new Exception("Már be vagy jelentkezve ezzel a fiókkal!");
                }
                foreach (User user in userList)
                {
                    if (user.UserName == username && user.Password == password && user.UserName != "Deleted;")
                    {
                        if (File.ReadAllLines("currentLogin.txt").Length != 0 && doLoginPopup)
                        {
                            if (MessageBox.Show("Biztosan be akarsz lépni egy másik fiókkal?", "Megerősítés", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                            {
                                return;
                            }
                        }
                        currUser = user;
                        if (doLoginPopup)
                        {
                            MessageBox.Show("Sikeres bejelentkezés: " + user.UserName);
                        }
                        File.WriteAllText("currentLogin.txt", user.ID + ";" + EncryptString(user.UserName, user.unKey) + ";" + EncryptString(user.Password, user.pwKey) + ";" + EncryptString(user.Email.Split('@', '.')[0], user.emKey.Split('@', '.')[0], 0) + "@" + EncryptString(user.Email.Split('@', '.')[1], user.emKey.Split('@', '.')[1], 1) + "." + EncryptString(user.Email.Split('@', '.')[2], user.emKey.Split('@', '.')[2], 2) + ";" + user.unKey + ";" + user.pwKey + ";" + user.emKey + ";" + user.Perms + ";");
                        isLoginSuccessful = true;
                    }
                }
                if (!isLoginSuccessful)
                {
                    throw new Exception("Sikertelen bejelentkezés, próbáld újra.");
                }
            }
            catch (Exception e)
            {
                File.SetAttributes("userData.txt", FileAttributes.Hidden);
                File.SetAttributes("keys.txt", FileAttributes.Hidden);
                File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                MessageBox.Show(e.Message);
            }
            File.SetAttributes("userData.txt", FileAttributes.Hidden);
            File.SetAttributes("keys.txt", FileAttributes.Hidden);
            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
        }
        //kijelentkezik a jelenlegi fiókból, az onDeletion az mindig false kivéve a törlő functionnek
        public static void LogOut(bool onDeletion = false)
        {
            File.SetAttributes("userData.txt", FileAttributes.Normal);
            File.SetAttributes("keys.txt", FileAttributes.Normal);
            File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
            try
            {
                if (currUser != null)
                {
                    if (!onDeletion)
                    {
                        if (MessageBox.Show("Biztosan ki akarsz jelentkezni?", "Kijelentkezés", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            currUser = null;
                            File.WriteAllText("currentLogin.txt", string.Empty);
                            MessageBox.Show("Sikeres kijelentkezés.");
                        }
                    }
                    else
                    {
                        currUser = null;
                        File.WriteAllText("currentLogin.txt", string.Empty);
                    }
                }
                else
                {
                    MessageBox.Show("Nem vagy bejelentkezve egy fiókba sem.");
                }
            }
            catch (Exception e)
            {
                File.SetAttributes("userData.txt", FileAttributes.Hidden);
                File.SetAttributes("keys.txt", FileAttributes.Hidden);
                File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                MessageBox.Show(e.Message);
            }
            File.SetAttributes("userData.txt", FileAttributes.Hidden);
            File.SetAttributes("keys.txt", FileAttributes.Hidden);
            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
        }
        //eltávolít egy fiókot
        private protected static bool DeleteUser([Optional] string deleteableUser)
        {           
            User tempUser = null;
            try
            {
                bool l = false;
                if (deleteableUser == userList[0].UserName)
                {
                    throw new Exception("Ezt az fiókot nem lehet törölni.");
                }
                if (deleteableUser != null)
                {
                    if (currUser.Perms == "a")
                    {
                        tempUser = currUser;
                        foreach (User x in userList)
                        {
                            if (x.ID == deleteableUser)
                            {
                                currUser = x;
                                l = true;
                            }
                        }
                        if (!l)
                        {
                            throw new Exception("Ilyen nevű fiók nincs!");
                        }
                    }
                    else
                    {
                        throw new Exception("Nincs engedélyed ehhez!");
                    }
                }
                if (currUser != null)
                {
                    if (tempUser != null && tempUser.Perms == "a" && tempUser.Password == new InputBox("Add meg a jelszót ismét", "", "Arial", 16).ShowDialog() || tempUser == null && currUser.Password == new InputBox("Add meg a jelszót ismét", "", "Arial", 16).ShowDialog())
                    {
                        if (MessageBox.Show("Biztos vagy benne, hogy törölni akarod ezt a fiókot?", "Megerősítés", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            File.SetAttributes("userData.txt", FileAttributes.Normal);
                            File.SetAttributes("keys.txt", FileAttributes.Normal);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
                            string[] x = File.ReadAllLines("userData.txt");
                            string[] y = File.ReadAllLines("keys.txt");
                            x[Convert.ToInt32(currUser.ID)] = "Deleted;";
                            y[Convert.ToInt32(currUser.ID)] = "Deleted;";
                            File.WriteAllLines("userData.txt", x);
                            File.WriteAllLines("keys.txt", y);
                            if (deleteableUser == null)
                            {
                                currUser = tempUser;
                            }
                            else
                            {
                                LogOut(true);
                            }
                            GetUsers();
                            File.SetAttributes("userData.txt", FileAttributes.Hidden);
                            File.SetAttributes("keys.txt", FileAttributes.Hidden);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                            currUser = tempUser;
                            MessageBox.Show("Fiók törölve.");
                            return true;
                        }
                        else
                        {
                            throw new Exception("");
                        }
                    }
                    else
                    {
                        throw new Exception("");
                    }
                }
                else
                {
                    throw new Exception("Jelentkezz be, mielőtt törölni akarsz egy fiókot!");
                }
            }
            catch (Exception e)
            {
                File.SetAttributes("userData.txt", FileAttributes.Hidden);
                File.SetAttributes("keys.txt", FileAttributes.Hidden);
                File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                if (e.Message != string.Empty)
                {
                    MessageBox.Show(e.Message);
                }
                currUser = tempUser;
                return false;
            }
        }
        //titkosít egy stringet
        private protected static string EncryptString(string pw, string key, [Optional] int a)
        {
            if (pw.Contains('@') && !string.IsNullOrEmpty(a.ToString()))
            {
                pw = pw.Split('@', '.')[a];
                key = key.Split('@', '.')[a];
            }
            string temp = "";
            for (int i = 0; i < pw.Length; i++)
            {
                temp += encrTable[Array.IndexOf(chars, pw[i]), Array.IndexOf(chars, key[i])];
            }
            temp = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(temp));
            return temp;
        }
        //visszafejt egy stringet, az "a" az email getterjéé
        private protected static string DecryptString(string encrPw, string key, [Optional] int a)
        {
            if (encrPw.Contains('@') && !string.IsNullOrEmpty(a.ToString()))
            {
                encrPw = encrPw.Split('@', '.')[a];
                key = key.Split('@', '.')[a];
            }
            encrPw = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(encrPw));
            string temp = "";
            for (int i = 0; i < encrPw.Length; i++)
            {
                if (Array.IndexOf(chars, encrPw[i]) - Array.IndexOf(chars, key[i]) < 0)
                {
                    temp += encrTable[Array.IndexOf(chars, encrPw[i]) - Array.IndexOf(chars, key[i]) + 62, 0];
                }
                else
                {
                    temp += encrTable[Array.IndexOf(chars, encrPw[i]) - Array.IndexOf(chars, key[i]), 0];
                }
            }
            return temp;
        }
        //megnézi, hogy valid email cím-e
        private protected static bool isEmailAddress(string email)
        {
            try
            {
                if (email.Count(a => a == '@') == 1 && email.Count(a => a == '.') == 1 && email.IndexOf('@') < email.IndexOf('.'))
                {
                    string[] n = email.Split('@', '.');

                    if (n[0].Length > 1 && n[1].Length > 1 && n[2].Length > 1)
                    {
                        for (int i = 0; i < n.Length; i++)
                        {
                            foreach (char c in n[i])
                            {
                                if (!chars.Contains(c))
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                return false;
            }
        }
        public static void EditUser()
        {
            try
            {
                if (currUser == null)
                {
                    throw new Exception("Nem vagy belépve egy fiókba sem.");
                }
                if (currUser.Perms == "a")
                {
                    new GridBox(0);
                }
                else
                {
                    new GridBox(1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        //netről lopott class egy text bemenetes messageboxra, kicsit átalakítva lol
        internal class InputBox : Window
        {

            Window Box = new Window();
            FontFamily font = new FontFamily("Tahoma");
            new int FontSize = 30;
            StackPanel sp1 = new StackPanel();
            string title = "Megerősítés";
            string boxcontent;
            string defaulttext = "";
            string errormessage = "Add meg a jelszót helyesen!";
            string errortitle = "Hiba";
            string okbuttontext = "OK";
            string cancelbuttontext = "Mégse";
            Brush BoxBackgroundColor = Brushes.Beige;
            Brush InputBackgroundColor = Brushes.White;
            //bool clicked = false;
            PasswordBox input = new PasswordBox();
            Button ok = new Button();
            Button cancel = new Button();
            bool inputreset = false;
            internal protected InputBox(string content)
            {
                try
                {
                    boxcontent = content;
                }
                catch { boxcontent = "Error!"; }
                windowdef();
            }
            internal protected InputBox(string content, string Htitle, string DefaultText)
            {
                try
                {
                    boxcontent = content;
                }
                catch { boxcontent = "Error!"; }
                try
                {
                    title = Htitle;
                }
                catch
                {
                    title = "Error!";
                }
                try
                {
                    defaulttext = DefaultText;
                }
                catch
                {
                    DefaultText = "Error!";
                }
                windowdef();
            }
            internal protected InputBox(string content, string Htitle, string Font, int Fontsize)
            {
                try
                {
                    boxcontent = content;
                }
                catch { boxcontent = "Error!"; }
                try
                {
                    font = new FontFamily(Font);
                }
                catch { font = new FontFamily("Tahoma"); }
                try
                {
                    title = Htitle;
                }
                catch
                {
                    title = "Error!";
                }
                if (Fontsize >= 1)
                    FontSize = Fontsize;
                windowdef();
            }
            private void windowdef()
            {
                Box.Height = 150;
                Box.Width = 230;
                Box.Background = BoxBackgroundColor;
                Box.Title = title;
                Box.Content = sp1;
                Box.Closing += Box_Closing;
                TextBlock content = new TextBlock();
                content.TextWrapping = TextWrapping.Wrap;
                content.Background = null;
                content.HorizontalAlignment = HorizontalAlignment.Center;
                content.Text = boxcontent;
                content.FontFamily = font;
                content.FontSize = FontSize;
                sp1.Children.Add(content);

                input.Background = InputBackgroundColor;
                input.FontFamily = font;
                input.FontSize = FontSize;
                input.HorizontalAlignment = HorizontalAlignment.Center;
                input.Password = defaulttext;
                input.MinWidth = 200;
                input.MouseEnter += input_MouseDown;
                sp1.Children.Add(input);
                ok.Width = 70;
                ok.Height = 30;
                ok.Click += ok_Click;
                ok.Content = okbuttontext;
                ok.HorizontalAlignment = HorizontalAlignment.Center;
                sp1.Children.Add(ok);
                cancel.Width = 70;
                cancel.Height = 30;
                cancel.Click += cancel_click;
                cancel.Content = cancelbuttontext;
                cancel.HorizontalAlignment = HorizontalAlignment.Center;
                sp1.Children.Add(cancel);

            }
            private void Box_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                /*if (!clicked)
                    e.Cancel = true;*/
            }
            private void input_MouseDown(object sender, MouseEventArgs e)
            {
                if ((sender as PasswordBox).Password == defaulttext && inputreset == false)
                {
                    (sender as PasswordBox).Password = null;
                    inputreset = true;
                }
            }
            private void cancel_click(object sender, RoutedEventArgs e)
            {
                Box.Close();
            }
            private void ok_Click(object sender, RoutedEventArgs e)
            {
                //clicked = true;
                if (input.Password == defaulttext || input.Password == "")
                {
                    MessageBox.Show(errormessage, errortitle);
                }
                else
                {
                    Box.Close();
                }
                //clicked = false;
            }
            new internal protected string ShowDialog()
            {
                Box.ShowDialog();
                return input.Password;
            }
        }
        //editelésre grid box, ezt már én csináltam teljesen lol
        internal class GridBox : Window
        {
            Window window = new Window();
            DataGrid grid = new DataGrid();
            StackPanel panel = new StackPanel();
            private FrameworkElementFactory tb1 = new FrameworkElementFactory(typeof(TextBox));
            private FrameworkElementFactory tb2 = new FrameworkElementFactory(typeof(TextBox));
            private DataGridTextColumn IDColumn = new DataGridTextColumn() { Header = "ID", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTemplateColumn userNameColumn = new DataGridTemplateColumn() { Header = "Felhasználónév", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTemplateColumn passwordColumn = new DataGridTemplateColumn() { Header = "Jelszó", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTemplateColumn emailColumn = new DataGridTemplateColumn() { Header = "Email cím", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTemplateColumn permsColumn = new DataGridTemplateColumn() { Header = "Jogosultságok", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTemplateColumn deleteColumn = new DataGridTemplateColumn() { Header = "Fiók törlése", Width = DataGridLength.Auto, IsReadOnly = true };
            private static string currentCellValue = "";
            internal GridBox(int x)
            {
                window.Width = 510;
                window.Height = 300;
                window.Background = Brushes.Wheat;
                grid.Background = Brushes.Cornsilk;
                grid.RowBackground = Brushes.Beige;
                grid.AutoGenerateColumns = false;
                grid.Width = 500;
                grid.Height = 300;
                grid.Margin = new Thickness(0, 0, 0, 0);
                grid.AlternatingRowBackground = Brushes.Beige;
                grid.SetBinding(DataGrid.SelectedItemProperty, new Binding("currentString") { Source = grid, Mode = BindingMode.OneWayToSource });
                grid.DataContext = userList;
                grid.ItemsSource = userList;
                switch (x)
                {
                    case 0:
                        grid.Loaded -= grid_Loaded_1;
                        grid.Loaded += grid_Loaded_0;
                        break;
                    case 1:
                        grid.Loaded -= grid_Loaded_0;
                        grid.Loaded += grid_Loaded_1;
                        break;
                }
                grid.HorizontalAlignment = HorizontalAlignment.Left;
                panel.Children.Add(grid);
                window.Content = panel;
                window.Show();
            }
            private void grid_Loaded_0(object sender, RoutedEventArgs e)
            {
                window.Title = "Fiókok beállításainak módosítása (Felhasználó:" + currUser.UserName + ")";
                Binding b = new Binding();
                b.Mode = BindingMode.TwoWay;
                b.ValidatesOnExceptions = true;
                b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                DataTemplate dtt = new DataTemplate();

                b = new Binding("ID");
                IDColumn.Binding = b;

                b = new Binding("UserName");
                dtt = new DataTemplate();
                tb1.SetBinding(TextBox.TextProperty, b);
                tb1.SetValue(TextBox.BackgroundProperty, Brushes.Beige);
                tb1.AddHandler(TextBox.GotFocusEvent, new RoutedEventHandler(tb1_GotFocus));
                tb1.AddHandler(TextBox.LostFocusEvent, new RoutedEventHandler(tb1_LostFocus));
                tb1.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(tb_TextChanged));
                dtt.VisualTree = tb1;
                userNameColumn.CellTemplate = dtt;

                dtt = new DataTemplate();
                FrameworkElementFactory button = new FrameworkElementFactory(typeof(Button));
                button.AddHandler(Button.ClickEvent, new RoutedEventHandler(button_Clicked));
                button.SetValue(Button.ContentProperty, "Jelszó változtatása");
                dtt.VisualTree = button;
                passwordColumn.CellTemplate = dtt;

                b = new Binding("Email");
                dtt = new DataTemplate();
                tb2.SetBinding(TextBox.TextProperty, b);
                tb2.SetValue(TextBox.BackgroundProperty, Brushes.Beige);
                tb2.AddHandler(TextBox.GotFocusEvent, new RoutedEventHandler(tb2_GotFocus));
                tb2.AddHandler(LostFocusEvent, new RoutedEventHandler(tb2_LostFocus));
                tb2.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(tb_TextChanged));
                dtt.VisualTree = tb2;
                emailColumn.CellTemplate = dtt;

                if (currUser.ID == "0")
                {
                    b = new Binding() { Path = new PropertyPath("Perms"), Mode = BindingMode.OneWay };
                    dtt = new DataTemplate();
                    FrameworkElementFactory comboBox1 = new FrameworkElementFactory(typeof(ComboBox));                    
                    comboBox1.SetValue(ComboBox.ItemsSourceProperty, new string[] { "a", "n" });
                    comboBox1.SetValue(ComboBox.SelectedItemProperty, b);
                    comboBox1.AddHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(ComboBoxSelectionChanged));
                    dtt.VisualTree = comboBox1;
                    permsColumn.CellTemplate = dtt;
                }
                else
                {
                    b = new Binding() { Path = new PropertyPath("Perms"), Mode = BindingMode.OneWay };
                    dtt = new DataTemplate();
                    FrameworkElementFactory label1 = new FrameworkElementFactory(typeof(Label));
                    label1.SetValue(Label.ContentProperty, b);
                    dtt.VisualTree = label1;
                    permsColumn.CellTemplate = dtt;
                }

                dtt = new DataTemplate();
                FrameworkElementFactory button2 = new FrameworkElementFactory(typeof(Button));
                button2.AddHandler(Button.ClickEvent, new RoutedEventHandler(button2_Clicked));
                button2.SetValue(Button.ContentProperty, "Törlés");
                dtt.VisualTree = button2;
                deleteColumn.CellTemplate = dtt;


                grid.Columns.Add(IDColumn);
                grid.Columns.Add(userNameColumn);
                grid.Columns.Add(passwordColumn);
                grid.Columns.Add(emailColumn);
                grid.Columns.Add(permsColumn);
                grid.Columns.Add(deleteColumn);
            }
            private void grid_Loaded_1(object sender, RoutedEventArgs e)
            {
                window.Title = "Fiók adatainak módosítása (Felhasználó:" + currUser.UserName + ")";
                grid.ItemsSource = userList.Where(x => x.ID == currUser.ID);
                Binding b = new Binding();
                b.Mode = BindingMode.TwoWay;
                b.ValidatesOnExceptions = true;
                b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                DataTemplate dtt = new DataTemplate();

                b = new Binding("UserName");
                dtt = new DataTemplate();
                tb1.SetBinding(TextBox.TextProperty, b);
                tb1.SetValue(TextBox.BackgroundProperty, Brushes.Beige);
                tb1.AddHandler(TextBox.GotFocusEvent, new RoutedEventHandler(tb1_GotFocus));
                tb1.AddHandler(TextBox.LostFocusEvent, new RoutedEventHandler(tb1_LostFocus));
                dtt.VisualTree = tb1;
                userNameColumn.CellTemplate = dtt;

                dtt = new DataTemplate();
                FrameworkElementFactory button = new FrameworkElementFactory(typeof(Button));
                button.AddHandler(Button.ClickEvent, new RoutedEventHandler(button_Clicked));
                button.SetValue(Button.ContentProperty, "Jelszó változtatása");
                dtt.VisualTree = button;
                passwordColumn.CellTemplate = dtt;

                b = new Binding("Email");
                dtt = new DataTemplate();
                tb2.SetBinding(TextBox.TextProperty, b);
                tb2.SetValue(TextBox.BackgroundProperty, Brushes.Beige);
                tb2.AddHandler(TextBox.GotFocusEvent, new RoutedEventHandler(tb2_GotFocus));
                tb2.AddHandler(LostFocusEvent, new RoutedEventHandler(tb2_LostFocus));
                dtt.VisualTree = tb2;
                emailColumn.CellTemplate = dtt;

                dtt = new DataTemplate();
                FrameworkElementFactory button2 = new FrameworkElementFactory(typeof(Button));
                button2.AddHandler(Button.ClickEvent, new RoutedEventHandler(button2_Clicked));
                button2.SetValue(Button.ContentProperty, "Törlés");
                dtt.VisualTree = button2;
                deleteColumn.CellTemplate = dtt;

                grid.Columns.Add(userNameColumn);
                grid.Columns.Add(passwordColumn);
                grid.Columns.Add(emailColumn);
                grid.Columns.Add(deleteColumn);
            }
            private void button_Clicked(object sender, RoutedEventArgs e)
            {                
                try
                {
                    if (currUser.Perms == "a")
                    {
                        if (grid.SelectedIndex == 0 && currUser.ID != "0")
                        {
                            throw new Exception("Nem módosíthatod ezt a fiókot.");
                        }
                        if (currUser.Password == new InputBox("Add meg a jelszót ismét", "Megerősítés", "Arial", 16).ShowDialog())
                        {
                            string tempPw = new InputBox("Írd be az új jelszót", "Jelszó módosítása", "Arial", 16).ShowDialog();
                            string tempKey = "";
                            if (tempPw.Length < 6 || tempPw.Length > 32)
                            {
                                throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                            }
                            if (tempPw == currUser.UserName)
                            {
                                throw new Exception("A felhasználónév és a jelszó nem egyezhet meg!");
                            }
                            foreach (char c in tempPw)
                            {
                                if (!chars.Contains(c))
                                {
                                    throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                                }
                            }
                            int len = 0;
                            while (len < tempPw.Length)
                            {
                                tempKey += chars[rnd.Next(0, 61)].ToString();
                                len++;
                            }
                            if (tempPw != null && MessageBox.Show("Biztos vagy benne, hogy megváltoztatod a fiók jelszavát?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                userList[grid.SelectedIndex].PW = tempPw;
                                userList[grid.SelectedIndex].pwKey = tempKey;
                                File.SetAttributes("userData.txt", FileAttributes.Normal);
                                File.SetAttributes("keys.txt", FileAttributes.Normal);
                                File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
                                string[] x = File.ReadAllLines("userData.txt");
                                string[] y = File.ReadAllLines("keys.txt");
                                for (int i = 0; i < x.Length; i++)
                                {
                                    if (userList[grid.SelectedIndex].ID == x[i].Split(';')[0])
                                    {
                                        x[i] = userList[grid.SelectedIndex].ID + ";" + EncryptString(userList[grid.SelectedIndex].UserName, userList[grid.SelectedIndex].unKey) + ";" + EncryptString(userList[grid.SelectedIndex].Password, userList[grid.SelectedIndex].pwKey) + ";" + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[0], userList[grid.SelectedIndex].emKey.Split('@', '.')[0]) + "@" + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[1], userList[grid.SelectedIndex].emKey.Split('@', '.')[1]) + "." + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[2], userList[grid.SelectedIndex].emKey.Split('@', '.')[2]) + ";";
                                        y[i] = userList[grid.SelectedIndex].ID + ";" + userList[grid.SelectedIndex].unKey + ";" + userList[grid.SelectedIndex].pwKey + ";" + userList[grid.SelectedIndex].emKey + ";" + userList[grid.SelectedIndex].Perms + ";";
                                    }
                                }
                                File.WriteAllLines("userData.txt", x);
                                File.WriteAllLines("keys.txt", y);
                                GetUsers();
                                File.SetAttributes("userData.txt", FileAttributes.Hidden);
                                File.SetAttributes("keys.txt", FileAttributes.Hidden);
                                File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                                MessageBox.Show("Sikeres módosítások.");
                            }
                        }
                        else
                        {
                            File.SetAttributes("userData.txt", FileAttributes.Hidden);
                            File.SetAttributes("keys.txt", FileAttributes.Hidden);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                            throw new Exception("Nem megfelelő jelszót adtál meg.");
                        }
                    }
                    else
                    {
                        if (currUser.Password == new InputBox("Add meg a jelszót ismét", "Megerősítés", "Arial", 16).ShowDialog())
                        {
                            string tempPw = new InputBox("Írd be az új jelszavad", "Jelszó módosítása", "Arial", 16).ShowDialog();
                            string tempKey = "";
                            if (tempPw.Length < 6 || tempPw.Length > 32)
                            {
                                throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                            }
                            if (tempPw == currUser.UserName)
                            {
                                throw new Exception("A felhasználónév és a jelszó nem egyezhet meg!");
                            }
                            foreach (char c in tempPw)
                            {
                                if (!chars.Contains(c))
                                {
                                    throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                                }
                            }
                            int len = 0;
                            while (len < tempPw.Length)
                            {
                                tempKey += chars[rnd.Next(0, 61)].ToString();
                                len++;
                            }
                            if (tempPw != null && MessageBox.Show("Biztos vagy benne, hogy megváltoztatnád a jelszavad?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                currUser.PW = tempPw;
                                currUser.pwkey = tempKey;
                                File.SetAttributes("userData.txt", FileAttributes.Normal);
                                File.SetAttributes("keys.txt", FileAttributes.Normal);
                                File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
                                string[] x = File.ReadAllLines("userData.txt");
                                string[] y = File.ReadAllLines("keys.txt");
                                string z = File.ReadAllLines("currentLogin.txt")[0];
                                for (int i = 0; i < x.Length; i++)
                                {
                                    if (currUser.ID == x[i].Split(';')[0])
                                    {
                                        x[i] = currUser.ID + ";" + EncryptString(currUser.UserName, currUser.unKey) + ";" + EncryptString(currUser.Password, currUser.pwKey) + ";" + EncryptString(currUser.Email.Split('@', '.')[0], currUser.emKey.Split('@', '.')[0]) + "@" + EncryptString(currUser.Email.Split('@', '.')[1], currUser.emKey.Split('@', '.')[1]) + "." + EncryptString(currUser.Email.Split('@', '.')[2], currUser.emKey.Split('@', '.')[2]) + ";";
                                        y[i] = currUser.ID + ";" + currUser.unKey + ";" + currUser.pwKey + ";" + currUser.emKey + ";" + currUser.Perms + ";";
                                        z = currUser.ID + ";" + EncryptString(currUser.UserName, currUser.unKey) + ";" + EncryptString(currUser.Password, currUser.pwKey) + ";" + EncryptString(currUser.Email.Split('@', '.')[0], currUser.emKey.Split('@', '.')[0], 0) + "@" + EncryptString(currUser.Email.Split('@', '.')[1], currUser.emKey.Split('@', '.')[1], 1) + "." + EncryptString(currUser.Email.Split('@', '.')[2], currUser.emKey.Split('@', '.')[2], 2) + ";" + currUser.unKey + ";" + currUser.pwKey + ";" + currUser.emKey + ";" + currUser.Perms + ";";
                                    }
                                }
                                File.WriteAllLines("userData.txt", x);
                                File.WriteAllLines("keys.txt", y);
                                File.WriteAllText("currentLogin.txt", z);
                                GetUsers();
                                File.SetAttributes("userData.txt", FileAttributes.Hidden);
                                File.SetAttributes("keys.txt", FileAttributes.Hidden);
                                File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                                MessageBox.Show("Sikeres módosítások.");
                            }
                        }
                        else
                        {
                            File.SetAttributes("userData.txt", FileAttributes.Hidden);
                            File.SetAttributes("keys.txt", FileAttributes.Hidden);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                            throw new Exception("Nem megfelelő jelszót adtál meg.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    File.SetAttributes("userData.txt", FileAttributes.Hidden);
                    File.SetAttributes("keys.txt", FileAttributes.Hidden);
                    File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                    MessageBox.Show(ex.Message);
                }
            }
            private void button2_Clicked(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (grid.SelectedIndex == 0)
                    {
                        throw new Exception("Ezt a fiókot nem lehet törölni.");
                    }
                    if (currUser.Perms == "a")
                    {
                        if (currUser.Perms == "a" && userList[grid.SelectedIndex].ID == currUser.ID || currUser.ID == "0" || userList[grid.SelectedIndex].Perms == "n")
                        {
                            if (DeleteUser(userList[grid.SelectedIndex].ID))
                            {                                
                                currentCellValue = "";
                                grid.ItemsSource = null;
                                grid.ItemsSource = userList;
                            }
                        }
                        else
                        {
                            throw new Exception("Nem törölhetsz másik admin fiókot!");
                        }
                    }
                    else
                    {
                        DeleteUser();
                        window.Close();
                    }
                }
                catch (Exception ex)
                {
                    File.SetAttributes("userData.txt", FileAttributes.Hidden);
                    File.SetAttributes("keys.txt", FileAttributes.Hidden);
                    File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                    if (ex.Message != string.Empty)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
            {               
                try
                {
                    if (grid.SelectedItem == null || string.IsNullOrEmpty(grid.SelectedItem.ToString()))
                    {
                        throw new Exception("");
                    }
                    if (grid.SelectedIndex == 0)
                    {
                        ((ComboBox)sender).SelectedItem = "a";
                        throw new Exception("Nem módosíthatod a saját jogosultságaidat.");
                        
                    }                   
                    ComboBox comboBox = sender as ComboBox;
                    if (comboBox != null)
                    {
                        userList[grid.SelectedIndex].Perms = comboBox.SelectedItem.ToString();
                        File.SetAttributes("userData.txt", FileAttributes.Normal);
                        File.SetAttributes("keys.txt", FileAttributes.Normal);
                        File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
                        string[] x = File.ReadAllLines("userData.txt");
                        string[] y = File.ReadAllLines("keys.txt");
                        for (int i = 0; i < x.Length; i++)
                        {
                            if (userList[grid.SelectedIndex].ID == x[i].Split(';')[0])
                            {
                                x[i] = userList[grid.SelectedIndex].ID + ";" + EncryptString(userList[grid.SelectedIndex].UserName, userList[grid.SelectedIndex].unKey) + ";" + EncryptString(userList[grid.SelectedIndex].Password, userList[grid.SelectedIndex].pwKey) + ";" + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[0], userList[grid.SelectedIndex].emKey.Split('@', '.')[0]) + "@" + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[1], userList[grid.SelectedIndex].emKey.Split('@', '.')[1]) + "." + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[2], userList[grid.SelectedIndex].emKey.Split('@', '.')[2]) + ";";
                                y[i] = userList[grid.SelectedIndex].ID + ";" + userList[grid.SelectedIndex].unKey + ";" + userList[grid.SelectedIndex].pwKey + ";" + userList[grid.SelectedIndex].emKey + ";" + userList[grid.SelectedIndex].Perms + ";";
                            }
                        }
                        File.WriteAllLines("userData.txt", x);
                        File.WriteAllLines("keys.txt", y);
                        GetUsers();
                        if (userList[grid.SelectedIndex].ID == currUser.ID)
                        {
                            Login(currUser.UserName, currUser.Password, false);
                        }
                        File.SetAttributes("userData.txt", FileAttributes.Hidden);
                        File.SetAttributes("keys.txt", FileAttributes.Hidden);
                        File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                        MessageBox.Show("Sikeres módosítások.");
                    }
                }
                catch (Exception ex)
                {
                    File.SetAttributes("userData.txt", FileAttributes.Hidden);
                    File.SetAttributes("keys.txt", FileAttributes.Hidden);
                    File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                    if (!string.IsNullOrEmpty(ex.Message))
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                File.SetAttributes("userData.txt", FileAttributes.Hidden);
                File.SetAttributes("keys.txt", FileAttributes.Hidden);
                File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
            }
            private void tb_TextChanged(object sender, TextChangedEventArgs e)
            {
                if (currUser.ID != "0" && currUser.Perms == "a" && grid.SelectedIndex == 0)
                {
                    ((TextBox)sender).Text = currentCellValue;
                }
            }
            private void tb1_GotFocus(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (currUser.ID != "0" && grid.SelectedIndex == 0)
                    {
                        throw new Exception("Nem módosíthatod ezt a fiókot.");
                    }
                    currentCellValue = ((TextBox)sender).Text;
                }
                catch (Exception ex)
                {
                    if (ex.Message != null)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            private void tb1_LostFocus(object sender, RoutedEventArgs e)
            {                
                try
                {
                    if (currUser.ID != "0" && currUser.Perms == "a" && grid.SelectedIndex == 0)
                    {
                        throw new Exception("");
                    }
                    TextBox a = (TextBox)sender;
                    if (a.Text == currentCellValue)
                    {
                        throw new Exception("");
                    }
                    if (a.Text.Length < 6 || a.Text.Length > 32)
                    {
                        throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                    }
                    foreach (char c in a.Text)
                    {
                        if (!chars.Contains(c))
                        {
                            throw new Exception("Nem megfelelő felhasználónév és/vagy jelszó!\n(Csak a-z, A-Z, 0-9 karakterek szereplhetnek benne,\nvalamint minimum 6, maximum 32 karakter\nhosszúságúnak kell lenniük!)");
                        }
                    }
                    int len = 0;
                    string key = "";
                    while (len < a.Text.Length)
                    {
                        key += chars[rnd.Next(0, 61)].ToString();
                        len++;
                    }
                    if (currUser.Perms == "a")
                    {
                        if (a.Text != null && MessageBox.Show("Biztos vagy benne, hogy megváltoztatod a fiók felhasználónevét?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            userList[grid.SelectedIndex].userName = a.Text;
                            userList[grid.SelectedIndex].unkey = key;
                            File.SetAttributes("userData.txt", FileAttributes.Normal);
                            File.SetAttributes("keys.txt", FileAttributes.Normal);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
                            string[] x = File.ReadAllLines("userData.txt");
                            string[] y = File.ReadAllLines("keys.txt");
                            for (int i = 0; i < x.Length; i++)
                            {
                                if (userList[grid.SelectedIndex].ID == x[i].Split(';')[0])
                                {
                                    x[i] = userList[grid.SelectedIndex].ID + ";" + EncryptString(userList[grid.SelectedIndex].UserName, userList[grid.SelectedIndex].unKey) + ";" + EncryptString(userList[grid.SelectedIndex].Password, userList[grid.SelectedIndex].pwKey) + ";" + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[0], userList[grid.SelectedIndex].emKey.Split('@', '.')[0]) + "@" + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[1], userList[grid.SelectedIndex].emKey.Split('@', '.')[1]) + "." + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[2], userList[grid.SelectedIndex].emKey.Split('@', '.')[2]) + ";";
                                    y[i] = userList[grid.SelectedIndex].ID + ";" + userList[grid.SelectedIndex].unKey + ";" + userList[grid.SelectedIndex].pwKey + ";" + userList[grid.SelectedIndex].emKey + ";" + userList[grid.SelectedIndex].Perms + ";";
                                }
                            }
                            File.WriteAllLines("userData.txt", x);
                            File.WriteAllLines("keys.txt", y);
                            GetUsers();
                            if (userList[grid.SelectedIndex].ID == currUser.ID)
                            {
                                Login(currUser.UserName, currUser.Password, false);
                            }
                            File.SetAttributes("userData.txt", FileAttributes.Hidden);
                            File.SetAttributes("keys.txt", FileAttributes.Hidden);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                            MessageBox.Show("Sikeres módosítások.");
                        }
                    }
                    else
                    {
                        if (a.Text != null && MessageBox.Show("Biztos vagy benne, hogy megváltoztatnád a fiók felhasználónevét?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            currUser.userName = a.Text;
                            currUser.unkey = key;
                            File.SetAttributes("userData.txt", FileAttributes.Normal);
                            File.SetAttributes("keys.txt", FileAttributes.Normal);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
                            string[] x = File.ReadAllLines("userData.txt");
                            string[] y = File.ReadAllLines("keys.txt");
                            string z = File.ReadAllLines("currentLogin.txt")[0];
                            for (int i = 0; i < x.Length; i++)
                            {
                                if (currUser.ID == x[i].Split(';')[0])
                                {
                                    x[i] = currUser.ID + ";" + EncryptString(currUser.UserName, currUser.unKey) + ";" + EncryptString(currUser.Password, currUser.pwKey) + ";" + EncryptString(currUser.Email.Split('@', '.')[0], currUser.emKey.Split('@', '.')[0]) + "@" + EncryptString(currUser.Email.Split('@', '.')[1], currUser.emKey.Split('@', '.')[1]) + "." + EncryptString(currUser.Email.Split('@', '.')[2], currUser.emKey.Split('@', '.')[2]) + ";";
                                    y[i] = currUser.ID + ";" + currUser.unKey + ";" + currUser.pwKey + ";" + currUser.emKey + ";" + currUser.Perms + ";";
                                    z = currUser.ID + ";" + EncryptString(currUser.UserName, currUser.unKey) + ";" + EncryptString(currUser.Password, currUser.pwKey) + ";" + EncryptString(currUser.Email.Split('@', '.')[0], currUser.emKey.Split('@', '.')[0], 0) + "@" + EncryptString(currUser.Email.Split('@', '.')[1], currUser.emKey.Split('@', '.')[1], 1) + "." + EncryptString(currUser.Email.Split('@', '.')[2], currUser.emKey.Split('@', '.')[2], 2) + ";" + currUser.unKey + ";" + currUser.pwKey + ";" + currUser.emKey + ";" + currUser.Perms + ";";
                                }
                            }
                            File.WriteAllLines("userData.txt", x);
                            File.WriteAllLines("keys.txt", y);
                            File.WriteAllText("currentLogin.txt", z);
                            GetUsers();
                            Login(currUser.UserName, currUser.Password, false);
                            File.SetAttributes("userData.txt", FileAttributes.Hidden);
                            File.SetAttributes("keys.txt", FileAttributes.Hidden);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                            MessageBox.Show("Sikeres módosítások.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    File.SetAttributes("userData.txt", FileAttributes.Hidden);
                    File.SetAttributes("keys.txt", FileAttributes.Hidden);
                    File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                    tb1 = new FrameworkElementFactory(typeof(TextBox)) { Text = currentCellValue };
                    if (ex.Message != string.Empty)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                currentCellValue = "";
                if (currUser.Perms == "a")
                {
                    grid.ItemsSource = userList;
                }
                else
                {
                    grid.ItemsSource = userList.Where(x => x.ID == currUser.ID);
                }
            }
            private void tb2_GotFocus(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (currUser.ID != "0" && grid.SelectedIndex == 0)
                    {
                        throw new Exception("Nem módosíthatod ezt a fiókot.");
                    }
                    currentCellValue = ((TextBox)sender).Text;
                }
                catch (Exception ex)
                {
                    if (ex.Message != null)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            private void tb2_LostFocus(object sender, RoutedEventArgs e)
            {                
                try
                {
                    if (currUser.ID != "0" && currUser.Perms == "a" && grid.SelectedIndex == 0)
                    {
                        throw new Exception("");
                    }
                    TextBox a = (TextBox)sender;
                    if (a.Text == currentCellValue)
                    {
                        throw new Exception("");
                    }
                    if (isEmailAddress(a.Text) && currUser.Perms == "a")
                    {
                        string key = "";
                        int len = 0;
                        string[] n = a.Text.Split('@', '.');
                        for (int i = 0; i < n.Length; i++)
                        {
                            len = 0;
                            if (n[i].Length < 1)
                            {
                                throw new Exception("Nem megfelelő email cím formátum.\n\nHelyes formátum: x@y.z");
                            }
                            while (len < n[i].Length)
                            {
                                key += chars[rnd.Next(0, 61)].ToString();
                                len++;
                            }
                            switch (i)
                            {
                                case 0:
                                    key += "@";
                                    break;
                                case 1:
                                    key += ".";
                                    break;
                            }
                        }
                        if (a.Text != null && MessageBox.Show("Biztos vagy benne, hogy megváltoztatod a fiók email címét?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            userList[grid.SelectedIndex].email = a.Text;
                            userList[grid.SelectedIndex].emkey = key;
                            File.SetAttributes("userData.txt", FileAttributes.Normal);
                            File.SetAttributes("keys.txt", FileAttributes.Normal);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
                            string[] x = File.ReadAllLines("userData.txt");
                            string[] y = File.ReadAllLines("keys.txt");
                            for (int i = 0; i < x.Length; i++)
                            {
                                if (userList[grid.SelectedIndex].ID == x[i].Split(';')[0])
                                {
                                    x[i] = userList[grid.SelectedIndex].ID + ";" + EncryptString(userList[grid.SelectedIndex].UserName, userList[grid.SelectedIndex].unKey) + ";" + EncryptString(userList[grid.SelectedIndex].Password, userList[grid.SelectedIndex].pwKey) + ";" + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[0], userList[grid.SelectedIndex].emKey.Split('@', '.')[0]) + "@" + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[1], userList[grid.SelectedIndex].emKey.Split('@', '.')[1]) + "." + EncryptString(userList[grid.SelectedIndex].Email.Split('@', '.')[2], userList[grid.SelectedIndex].emKey.Split('@', '.')[2]) + ";";
                                    y[i] = userList[grid.SelectedIndex].ID + ";" + userList[grid.SelectedIndex].unKey + ";" + userList[grid.SelectedIndex].pwKey + ";" + userList[grid.SelectedIndex].emKey + ";" + userList[grid.SelectedIndex].Perms + ";";
                                }
                            }
                            File.WriteAllLines("userData.txt", x);
                            File.WriteAllLines("keys.txt", y);
                            GetUsers();
                            if (userList[grid.SelectedIndex].ID == currUser.ID)
                            {
                                Login(currUser.UserName, currUser.Password, false);
                            }
                            File.SetAttributes("userData.txt", FileAttributes.Hidden);
                            File.SetAttributes("keys.txt", FileAttributes.Hidden);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                            MessageBox.Show("Sikeres módosítások.");
                        }
                    }
                    else if (isEmailAddress(a.Text))
                    {
                        string key = "";
                        int len = 0;
                        string[] n = a.Text.Split('@', '.');
                        for (int i = 0; i < n.Length; i++)
                        {
                            len = 0;
                            if (n[i].Length < 1)
                            {
                                throw new Exception("Adj meg email címet!");
                            }
                            while (len < n[i].Length)
                            {
                                key += chars[rnd.Next(0, 61)].ToString();
                                len++;
                            }
                            switch (i)
                            {
                                case 0:
                                    key += "@";
                                    break;
                                case 1:
                                    key += ".";
                                    break;
                            }
                        }
                        if (a.Text != null && MessageBox.Show("Biztos vagy benne, hogy megváltoztatnád a fiók email címét??", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            currUser.email = a.Text;
                            currUser.emkey = key;
                            File.SetAttributes("userData.txt", FileAttributes.Normal);
                            File.SetAttributes("keys.txt", FileAttributes.Normal);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Normal);
                            string[] x = File.ReadAllLines("userData.txt");
                            string[] y = File.ReadAllLines("keys.txt");
                            string z = File.ReadAllLines("currentLogin.txt")[0];
                            for (int i = 0; i < x.Length; i++)
                            {
                                if (currUser.ID == x[i].Split(';')[0])
                                {
                                    x[i] = currUser.ID + ";" + EncryptString(currUser.UserName, currUser.unKey) + ";" + EncryptString(currUser.Password, currUser.pwKey) + ";" + EncryptString(currUser.Email.Split('@', '.')[0], currUser.emKey.Split('@', '.')[0]) + "@" + EncryptString(currUser.Email.Split('@', '.')[1], currUser.emKey.Split('@', '.')[1]) + "." + EncryptString(currUser.Email.Split('@', '.')[2], currUser.emKey.Split('@', '.')[2]) + ";";
                                    y[i] = currUser.ID + ";" + currUser.unKey + ";" + currUser.pwKey + ";" + currUser.emKey + ";" + currUser.Perms + ";";
                                    z = currUser.ID + ";" + EncryptString(currUser.UserName, currUser.unKey) + ";" + EncryptString(currUser.Password, currUser.pwKey) + ";" + EncryptString(currUser.Email.Split('@', '.')[0], currUser.emKey.Split('@', '.')[0], 0) + "@" + EncryptString(currUser.Email.Split('@', '.')[1], currUser.emKey.Split('@', '.')[1], 1) + "." + EncryptString(currUser.Email.Split('@', '.')[2], currUser.emKey.Split('@', '.')[2], 2) + ";" + currUser.unKey + ";" + currUser.pwKey + ";" + currUser.emKey + ";" + currUser.Perms + ";";
                                }
                            }
                            File.WriteAllLines("userData.txt", x);
                            File.WriteAllLines("keys.txt", y);
                            File.WriteAllText("currentLogin.txt", z);
                            GetUsers();
                            Login(currUser.UserName, currUser.Password, false);
                            File.SetAttributes("userData.txt", FileAttributes.Hidden);
                            File.SetAttributes("keys.txt", FileAttributes.Hidden);
                            File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                            MessageBox.Show("Sikeres módosítások.");
                        }
                    }
                    else
                    {
                        throw new Exception("Nem megfelelő email cím formátum.\n\nHelyes formátum: x@y.z");
                    }
                }
                catch (Exception ex)
                {
                    File.SetAttributes("userData.txt", FileAttributes.Hidden);
                    File.SetAttributes("keys.txt", FileAttributes.Hidden);
                    File.SetAttributes("currentLogin.txt", FileAttributes.Hidden);
                    tb2 = new FrameworkElementFactory(typeof(TextBox)) { Text = currentCellValue };
                    if (ex.Message != string.Empty)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                currentCellValue = "";
                grid.ItemsSource = null;
                if (currUser.Perms == "a")
                {
                    grid.ItemsSource = userList;
                }
                else
                {
                    grid.ItemsSource = userList.Where(x => x.ID == currUser.ID);
                }
            }
        }
    }
}
