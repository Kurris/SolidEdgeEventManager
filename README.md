# SolidEdgeEventManager
处理SolidEdge的常见事件,高度封装,适合对SE的二次封装

### 使用方式(Usage)
* 如果当前为窗体,传入当前窗体实例,则以该窗体为主键,对其增减不同的事件
```C#
SolidEdgeEventManager.Instance.AddOrReplaceEvent(this, SEEvent.ApplicationAfterActiveDocumentChange, x =>
{
      //TODO 
});
```
   建议在窗体基类中统一移除所有事件
```C#
      protected override void OnClosed(EventArgs e)
        {
            if (!DesignMode)
            {     
                //移除当前窗体所有事件
                SEEventManager.Instance.RemoveAllEvents(this);
            }
            base.OnClosed(e);
        }
 ```

* 如果在其他类中使用,可以依据开发人员给定的Key为依据
```C#
//Key可以为任意类型
SolidEdgeEventManager.Instance.AddOrReplaceEvent(Key, SEEvent.ApplicationAfterActiveDocumentChange, x =>
 {
      //TODO 
});
```
  
* 如果需要为SolidEdge中ASM文档下的par文件添加事件
```C#
SolidEdgeEventManager.Instance.AddEvent(this,"文件名", SEEvent.AssemblyRecomputeAfterRecompute,x => 
{
      //TODO
});
 ```
