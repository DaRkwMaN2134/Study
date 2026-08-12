using System.ComponentModel;
using System.Data.SqlTypes;

var intInput = new List<int> { 6, 6, 1, 2, 2, 3, 4, 4, 5 };
string strInput1 = "А роза упала на лапу Азора";
string strInput2 = "А роза упала на лапу Азора";

var result1 = RemoveDuplicates(intInput);
Console.WriteLine(string.Join(",", result1));

var result2 = IsPalindrome(strInput1);
Console.WriteLine(result2);


var result3 = CountWords(strInput2);
Console.WriteLine(result3);
List<T> RemoveDuplicates<T>(List<T> 
    list)
{
    HashSet<T> removeList = new HashSet<T> { };
    List<T> result = new List<T>();
    foreach (var item in list)
    {
        if (removeList.Add(item)) // вернёт true, если элемент новый
            result.Add(item);
    }
    return result;
}

bool IsPalindrome(string s)
{
    s = s.ToLower();
    int left = 0;
    int right = s.Length - 1;

    while (left < right)
    {
        if (s[left] == ' ')
        { 
            left++; 
            continue; 
        }

        if (s[right] == ' ') 
        { 
            right--; 
            continue; 
        }

        if (s[left] != s[right])
        {
            return false;
        }

        left++;
        right--;
    }
    return true;
}

int CountWords(string s)
{
    int count = 0;
    bool inWord = false;
    for (int i = 0; i<s.Length; i++)
    {
        if (s[i] == ' ')
        {
            inWord = false;
        }
        else if (inWord == false)
        {
            inWord = true;
            count++;
        }
    }
    return count;
}