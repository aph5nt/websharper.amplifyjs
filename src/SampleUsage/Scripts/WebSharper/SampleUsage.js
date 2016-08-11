(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,amplify,alert,String,SampleUsage,Client,PostModel,Arrays,List,UI,Next,Doc,T,AttrProxy,Key,ListModel,Unchecked,console;
 Runtime.Define(Global,{
  SampleUsage:{
   Client:{
    Main:function()
    {
     var postModel,arg20,arg201,arg202,arg203;
     amplify.subscribe("message",function(data)
     {
      return alert(String(data));
     });
     postModel=PostModel.Create();
     Client.getAll(function(posts)
     {
      var objectArg;
      postModel.Items.Clear();
      objectArg=postModel.Items;
      return Arrays.iter(function(arg00)
      {
       return objectArg.Add(arg00);
      },posts);
     });
     arg201=function()
     {
      return amplify.store("getAllPosts",null);
     };
     arg202=function()
     {
      amplify.publish.apply(amplify,["message","Hello AmplifyJS!"]);
     };
     arg203=List.ofArray([Doc.TextNode("Title")]);
     arg20=List.ofArray([Doc.Button("Clear Cache",Runtime.New(T,{
      $:0
     }),arg201),Doc.Button(" Publish Message",Runtime.New(T,{
      $:0
     }),arg202),Doc.Element("table",List.ofArray([AttrProxy.Create("class","table table-hover")]),List.ofArray([Doc.Element("th",[],arg203),Doc.Convert(function(post)
     {
      var arg204,arg205;
      arg205=List.ofArray([Doc.TextNode(post.title)]);
      arg204=List.ofArray([Doc.Element("td",[],arg205)]);
      return Doc.Element("tr",[],arg204);
     },postModel.Items.get_View())]))]);
     return Doc.Element("div",[],arg20);
    },
    PostModel:Runtime.Class({},{
     Bind:function(posts)
     {
      var arg00;
      arg00=function()
      {
       return Key.Fresh();
      };
      return Runtime.New(PostModel,{
       Items:ListModel.Create(arg00,posts)
      });
     },
     Create:function()
     {
      var arg00,arg10;
      arg00=function()
      {
       return Key.Fresh();
      };
      arg10=Runtime.New(T,{
       $:0
      });
      return Runtime.New(PostModel,{
       Items:ListModel.Create(arg00,arg10)
      });
     }
    }),
    cache:Runtime.Field(function()
    {
     var c;
     c={};
     c.number=1000;
     return{
      $:2,
      $0:c
     };
    }),
    createPost:function(post)
    {
     var settings,reqs;
     settings={};
     settings.url="https://jsonplaceholder.typicode.com/posts";
     settings.type="POST";
     settings.dataType="json";
     settings.data=post;
     amplify.request.define("createPost","ajax",settings);
     reqs={
      resourceId:"createPost"
     };
     reqs.success=function(msg)
     {
      return Client.log(msg);
     };
     return amplify.request(reqs);
    },
    getAll:function(callback)
    {
     var cache,_,settings,reqs;
     cache=amplify.store("getAllPosts");
     if(!Unchecked.Equals(cache,undefined))
      {
       _=callback(cache);
      }
     else
      {
       settings={};
       settings.url="https://jsonplaceholder.typicode.com/posts";
       settings.type="GET";
       settings.dataType="json";
       settings.cache=true;
       amplify.request.define("getAllPosts","ajax",settings);
       reqs={
        resourceId:"getAllPosts"
       };
       reqs.success=function(data)
       {
        callback(data);
        return amplify.store("getAllPosts",data);
       };
       _=amplify.request(reqs);
      }
     return _;
    },
    getById:function(callback)
    {
     var settings,args;
     settings={};
     settings.url="https://jsonplaceholder.typicode.com/posts/{id}";
     settings.type="GET";
     settings.dataType="json";
     settings.cache=Client.cache().$0;
     amplify.request.define("getPostById","ajax",settings);
     args={
      id:"1"
     };
     return amplify.request("getPostById",args,function(data)
     {
      return callback(data);
     });
    },
    log:function(msg)
    {
     return console?console.log(msg):undefined;
    }
   }
  }
 });
 Runtime.OnInit(function()
 {
  amplify=Runtime.Safe(Global.amplify);
  alert=Runtime.Safe(Global.alert);
  String=Runtime.Safe(Global.String);
  SampleUsage=Runtime.Safe(Global.SampleUsage);
  Client=Runtime.Safe(SampleUsage.Client);
  PostModel=Runtime.Safe(Client.PostModel);
  Arrays=Runtime.Safe(Global.WebSharper.Arrays);
  List=Runtime.Safe(Global.WebSharper.List);
  UI=Runtime.Safe(Global.WebSharper.UI);
  Next=Runtime.Safe(UI.Next);
  Doc=Runtime.Safe(Next.Doc);
  T=Runtime.Safe(List.T);
  AttrProxy=Runtime.Safe(Next.AttrProxy);
  Key=Runtime.Safe(Next.Key);
  ListModel=Runtime.Safe(Next.ListModel);
  Unchecked=Runtime.Safe(Global.WebSharper.Unchecked);
  return console=Runtime.Safe(Global.console);
 });
 Runtime.OnLoad(function()
 {
  Client.cache();
  return;
 });
}());
