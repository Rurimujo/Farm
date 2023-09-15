namespace MyFrameworkCore
{
    public class ConfigEvent
    {
        //场景相关
        public const string SwichConfinerShape = "切换场景边界";
        public const string SceneTransition = "场景传送";
        public const string PlayerMoveToPosition = "人物加载场景时候的坐标";
        public const string SceneBeforeUnload = "卸载场景之前需要做的事件";
        public const string SceneAfterLoaded = "加载场景之后需要做的事件";

        //UI界面相关
        public const string UIItemCreatOnWorld = "在世界地图生成物品";

        //场景过度相关
        public const string UIFade = "场景过度";
    }
}
