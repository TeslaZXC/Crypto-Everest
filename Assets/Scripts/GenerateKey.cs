using System;
using UnityEngine;

public class GenerateKey
{
    public static string CreateKey()
    {
        return string.Create(6, 0, (span, state) =>
        {
            for (int i = 0; i < 6; i++)
            {
                int randomNumber = UnityEngine.Random.Range(0, 36);

                if (randomNumber < 10)
                {
                    span[i] = (char)(randomNumber + 48);
                }
                else
                {
                    span[i] = (char)(randomNumber + 55);
                }
            }
        });
    }
}
