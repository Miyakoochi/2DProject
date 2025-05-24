using System.Collections.Generic;
using Common;
using QFramework;


namespace AssetSystem
{
    public interface IAddressableModel : IModel
    {

        /// <summary>
        /// 资产加载展示的总进度值
        /// </summary>
        public BindableProperty<float> ShowProcessValue { get; set; }
        
        /// <summary>
        /// 资产加载展示进度
        /// </summary>
        public BindableProperty<float> LoaderProcessValue { get; set; }

        /// <summary>
        /// 资源的所有Tag
        /// </summary>
        public List<string> Tags { get; set; }

        public long CurrentDownloadAssetSize { get; set; }

    }
    
    public class AddressableModel : AbstractModel, IAddressableModel
    {
        public BindableProperty<float> ShowProcessValue { get; set; } = new();
        public BindableProperty<float> LoaderProcessValue { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public long CurrentDownloadAssetSize { get; set; }

        protected override void OnInit()
        {
            Tags.AddRange(new List<string>()
            {
                Util.PlayerDataModelTag,
                Util.LevelDataModelTag
            });
        }
    }
}