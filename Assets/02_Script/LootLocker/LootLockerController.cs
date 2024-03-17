using LootLocker.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

public static class LootLockerController
{

    public static void Init(Action<bool> endCallback = null)
    {

        LootLockerSDKManager.StartGuestSession((res) =>
        {

            endCallback?.Invoke(res.success);

        });


    }

    public static void GetPlayerName(Action<string> endCallback = null)
    {

        LootLockerSDKManager.GetPlayerName((res) =>
        {

            if (res.success)
            {

                endCallback?.Invoke(res.name);

            }
            else
            {

                endCallback?.Invoke(null);

            }

        });

    }

    public static void SetPlayerName(string playerName, Action<bool> endCallback = null)
    {

        LootLockerSDKManager.SetPlayerName(playerName, (res) =>
        {

            endCallback?.Invoke(res.success);

        });

    }

    public static void UplodeScore(string id, int score, string leaderboardKey, Action<bool> endCallback = null)
    {

        LootLockerSDKManager.SubmitScore(id, score, leaderboardKey, (res) =>
        {

            endCallback?.Invoke(res.success);

        });

    }

    public static void GetLederboard(string id, int getCount, Action<List<LootLockerLeaderboardMember>> endCallback)
    {

        LootLockerSDKManager.GetScoreList(id, getCount, (res) =>
        {

            if (res.success)
            {

                endCallback?.Invoke(res.items.ToList());

            }
            else
            {

                endCallback?.Invoke(null);

            }

        });

    }


}