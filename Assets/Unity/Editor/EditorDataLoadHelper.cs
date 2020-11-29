using System.IO;
using Core.Etc;
using SadPumpkin.Util.Context;
using UnityEngine;

namespace Unity.Editor
{
    public static class EditorDataLoadHelper
    {
        public static IContext LoadEditorContext()
        {
            PathUtility pathUtility = new PathUtility(
                "editorUser",
                Path.Combine(Application.persistentDataPath, "SaveData"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Ability"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "PlayerClass"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "EnemyClass"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "CalamityClass"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "EnemyGroup"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Item"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Weapon"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Armor"));

            IContext context = new Context();
            context.Set(pathUtility);

            DataLoadHelper.LoadJsonSettings(context);
            DataLoadHelper.LoadDatabases(context);

            return context;
        }
    }
}