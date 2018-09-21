using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon
{
    /// <summary>
    /// 序列化工具接口
    /// </summary>
    public interface ISerializeTool
    {
        Byte[] Serialize(object obj);
        object Deserialize(Byte[] data, Type type);
    }
}
