using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using CommonLib;

namespace CommonLib.Entity
{
   public class User : ATable, INotifyPropertyChanged, ICloneable
   {
      #region Member Variables
      SnowflakeIdWorker snowflakeIdWorker = new SnowflakeIdWorker();
      private String userid_;
      private String dingdinguserid_;
      private String dingdingdeptid_;
      private String username_;
      private String account_;
      private String password_;
      private String changetime_;
      private String mobile_;
      private String workplace_;
      private String memo_;
      private String isdeleted_;
      #endregion
      #region Attributes
      public String UserID
      {
         get{ return userid_; }
         set{ userid_ = value; NotifyPropertyChanged("UserID");}
      }
      public String DingDingUserID
      {
         get{ return dingdinguserid_; }
         set{ dingdinguserid_ = value; NotifyPropertyChanged("DingDingUserID");}
      }
      public String DingDingDeptID
      {
         get{ return dingdingdeptid_; }
         set{ dingdingdeptid_ = value; NotifyPropertyChanged("DingDingDeptID");}
      }
      public String UserName
      {
         get{ return username_; }
         set{ username_ = value; NotifyPropertyChanged("UserName");}
      }
      public String Account
      {
         get{ return account_; }
         set{ account_ = value; NotifyPropertyChanged("Account");}
      }
      public String Password
      {
         get{ return password_; }
         set{ password_ = value; NotifyPropertyChanged("Password");}
      }
      public String ChangeTime
      {
         get{ return changetime_; }
         set{ changetime_ = value; NotifyPropertyChanged("ChangeTime");}
      }
      public String Mobile
      {
         get{ return mobile_; }
         set{ mobile_ = value; NotifyPropertyChanged("Mobile");}
      }
      public String WorkPlace
      {
         get{ return workplace_; }
         set{ workplace_ = value; NotifyPropertyChanged("WorkPlace");}
      }
      public String Memo
      {
         get{ return memo_; }
         set{ memo_ = value; NotifyPropertyChanged("Memo");}
      }
      public String IsDeleted
      {
         get{ return isdeleted_; }
         set{ isdeleted_ = value; }
      }
      #endregion 
      #region Constructors
      public User (): this("","","","","","","","","","",""){}
      public User (String userid_,String dingdinguserid_,String dingdingdeptid_,String username_,String account_,String password_,String changetime_,String mobile_,String workplace_,String memo_,String isdeleted_)
      {
         this.userid_ = userid_;//
         this.dingdinguserid_ = dingdinguserid_;//
         this.dingdingdeptid_ = dingdingdeptid_;//
         this.username_ = username_;//
         this.account_ = account_;//
         this.password_ = password_;//
         this.changetime_ = changetime_;//
         this.mobile_ = mobile_;//
         this.workplace_ = workplace_;//
         this.memo_ = memo_;//
         this.isdeleted_ = isdeleted_;//
      }
      public User (User user_)
      {
         this.userid_ = user_.userid_;//
         this.dingdinguserid_ = user_.dingdinguserid_;//
         this.dingdingdeptid_ = user_.dingdingdeptid_;//
         this.username_ = user_.username_;//
         this.account_ = user_.account_;//
         this.password_ = user_.password_;//
         this.changetime_ = user_.changetime_;//
         this.mobile_ = user_.mobile_;//
         this.workplace_ = user_.workplace_;//
         this.memo_ = user_.memo_;//
         this.isdeleted_ = user_.isdeleted_;//
      }
      #endregion 
      #region IModel Function
      public string GetAsString()
      {
         return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", this.userid_,this.dingdinguserid_,this.dingdingdeptid_,this.username_,this.account_,this.password_,this.changetime_,this.mobile_,this.workplace_,this.memo_,this.isdeleted_);
      }
      #endregion
      #region INotifyPropertyChanged Function
      public event PropertyChangedEventHandler PropertyChanged;
      public void NotifyPropertyChanged(string name)
      {
         if (PropertyChanged != null)
         {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
         }
      }
      #endregion
      #region ICloneable Function
      public object Clone()
      {
         return base.CloneEntity<User>(this);
      }
      public User DeepClone()
      {
         return (User)this.Clone();
      }
      #endregion ICloneable
      #region SetSQL Function
      /// <summary>
      /// 获取添加数据的SQL语句
      /// <summary>
      public  string SetInsert()      {
         string strSQL = "INSERT INTO [User] (UserID,DingDingUserID,DingDingDeptID,UserName,Account,Password,ChangeTime,Mobile,WorkPlace,Memo,IsDeleted) VALUES (@UserID,@DingDingUserID,@DingDingDeptID,@UserName,@Account,@Password,getdate(),@Mobile,@WorkPlace,@Memo,@IsDeleted)";
         UserID= snowflakeIdWorker.NextId().ToString();
                     SqlParameter[] sp = new SqlParameter[11];
            sp[0] = new SqlParameter("@UserID", UserID);
            sp[1] = new SqlParameter("@DingDingUserID", DingDingUserID);
            sp[2] = new SqlParameter("@DingDingDeptID", DingDingDeptID);
            sp[3] = new SqlParameter("@UserName", UserName);
            sp[4] = new SqlParameter("@Account", Account);
            sp[5] = new SqlParameter("@Password", Password);
            sp[6] = new SqlParameter("@ChangeTime", ChangeTime);
            sp[7] = new SqlParameter("@Mobile", Mobile);
            sp[8] = new SqlParameter("@WorkPlace", WorkPlace);
            sp[9] = new SqlParameter("@Memo", Memo);
            sp[10] = new SqlParameter("@IsDeleted", IsDeleted);

         new DbHelper().ExcuteNonQuery(strSQL,sp);
         return UserID;
      }
      public  string SetInsertForTransaction(DbHelper dbHelper)      {
         string strSQL = "INSERT INTO [User] (UserID,DingDingUserID,DingDingDeptID,UserName,Account,Password,ChangeTime,Mobile,WorkPlace,Memo,IsDeleted) VALUES (@UserID,@DingDingUserID,@DingDingDeptID,@UserName,@Account,@Password,getdate(),@Mobile,@WorkPlace,@Memo,@IsDeleted)";
         UserID= snowflakeIdWorker.NextId().ToString();
                     SqlParameter[] sp = new SqlParameter[11];
            sp[0] = new SqlParameter("@UserID", UserID);
            sp[1] = new SqlParameter("@DingDingUserID", DingDingUserID);
            sp[2] = new SqlParameter("@DingDingDeptID", DingDingDeptID);
            sp[3] = new SqlParameter("@UserName", UserName);
            sp[4] = new SqlParameter("@Account", Account);
            sp[5] = new SqlParameter("@Password", Password);
            sp[6] = new SqlParameter("@ChangeTime", ChangeTime);
            sp[7] = new SqlParameter("@Mobile", Mobile);
            sp[8] = new SqlParameter("@WorkPlace", WorkPlace);
            sp[9] = new SqlParameter("@Memo", Memo);
            sp[10] = new SqlParameter("@IsDeleted", IsDeleted);

         dbHelper.ExcuteNonQueryForTransaction(strSQL,sp);
         return UserID;
      }
      /// <summary>
      /// 更新数据
      /// <summary>
      public int SetUpdateForAll()
      {
         string strSQL = "UPDATE [User] SET "+
            "DingDingUserID= @DingDingUserID,"+
            "DingDingDeptID= @DingDingDeptID,"+
            "UserName= @UserName,"+
            "Account= @Account,"+
            "Password= @Password,"+
            "ChangeTime= getdate(),"+
            "Mobile= @Mobile,"+
            "WorkPlace= @WorkPlace,"+
            "Memo= @Memo,"+
            "IsDeleted= @IsDeleted  WHERE "+
            "UserID=@UserID "; 
            SqlParameter[] sp = new SqlParameter[11];
            sp[0] = new SqlParameter("@UserID", UserID);
            sp[1] = new SqlParameter("@DingDingUserID", DingDingUserID);
            sp[2] = new SqlParameter("@DingDingDeptID", DingDingDeptID);
            sp[3] = new SqlParameter("@UserName", UserName);
            sp[4] = new SqlParameter("@Account", Account);
            sp[5] = new SqlParameter("@Password", Password);
            sp[6] = new SqlParameter("@ChangeTime", ChangeTime);
            sp[7] = new SqlParameter("@Mobile", Mobile);
            sp[8] = new SqlParameter("@WorkPlace", WorkPlace);
            sp[9] = new SqlParameter("@Memo", Memo);
            sp[10] = new SqlParameter("@IsDeleted", IsDeleted);

         return new DbHelper().ExcuteNonQuery(strSQL,sp);
      }
      /// <summary>
      /// 更新部分数据
      /// <summary>
      public int SetUpdate()
      {
         bool boolPartial = false;
         string strSQL = "UPDATE [User] SET ";
            if (!String.IsNullOrEmpty(DingDingUserID))
            {
                  strSQL +="DingDingUserID= @DingDingUserID,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(DingDingDeptID))
            {
                  strSQL +="DingDingDeptID= @DingDingDeptID,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(UserName))
            {
                  strSQL +="UserName= @UserName,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(Account))
            {
                  strSQL +="Account= @Account,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(Password))
            {
                  strSQL +="Password= @Password,";
                  boolPartial = true;
            }
            ChangeTime = DateTime.Now.ToString();
            if (!String.IsNullOrEmpty(ChangeTime))
            {
                  strSQL +="ChangeTime= @ChangeTime,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(Mobile))
            {
                  strSQL +="Mobile= @Mobile,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(WorkPlace))
            {
                  strSQL +="WorkPlace= @WorkPlace,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(Memo))
            {
                  strSQL +="Memo= @Memo,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(IsDeleted))
            {
                   strSQL +="IsDeleted= @IsDeleted";
            }
            else
            {
                  if(boolPartial)
                  {
                     strSQL = strSQL.Remove(strSQL.Length - 1, 1);//去掉后面的逗号。
                  }
            }
             strSQL +=" WHERE "; 
            strSQL +="UserID=@UserID "; 
            SqlParameter[] sp = new SqlParameter[11];
            sp[0] = new SqlParameter("@UserID", UserID);
            sp[1] = new SqlParameter("@DingDingUserID", DingDingUserID);
            sp[2] = new SqlParameter("@DingDingDeptID", DingDingDeptID);
            sp[3] = new SqlParameter("@UserName", UserName);
            sp[4] = new SqlParameter("@Account", Account);
            sp[5] = new SqlParameter("@Password", Password);
            sp[6] = new SqlParameter("@ChangeTime", ChangeTime);
            sp[7] = new SqlParameter("@Mobile", Mobile);
            sp[8] = new SqlParameter("@WorkPlace", WorkPlace);
            sp[9] = new SqlParameter("@Memo", Memo);
            sp[10] = new SqlParameter("@IsDeleted", IsDeleted);

         return new DbHelper().ExcuteNonQuery(strSQL,sp);
      }
      public int SetUpdateForTransaction(DbHelper dbHelper)
      {
         bool boolPartial = false;
         string strSQL = "UPDATE [User] SET ";
            if (!String.IsNullOrEmpty(DingDingUserID))
            {
                  strSQL +="DingDingUserID= @DingDingUserID,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(DingDingDeptID))
            {
                  strSQL +="DingDingDeptID= @DingDingDeptID,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(UserName))
            {
                  strSQL +="UserName= @UserName,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(Account))
            {
                  strSQL +="Account= @Account,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(Password))
            {
                  strSQL +="Password= @Password,";
                  boolPartial = true;
            }
            ChangeTime = DateTime.Now.ToString();
            if (!String.IsNullOrEmpty(ChangeTime))
            {
                  strSQL +="ChangeTime= @ChangeTime,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(Mobile))
            {
                  strSQL +="Mobile= @Mobile,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(WorkPlace))
            {
                  strSQL +="WorkPlace= @WorkPlace,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(Memo))
            {
                  strSQL +="Memo= @Memo,";
                  boolPartial = true;
            }
            if (!String.IsNullOrEmpty(IsDeleted))
            {
                   strSQL +="IsDeleted= @IsDeleted";
            }
            else
            {
                  if(boolPartial)
                  {
                     strSQL = strSQL.Remove(strSQL.Length - 1, 1);//去掉后面的逗号。
                  }
            }
             strSQL +=" WHERE "; 
            strSQL +="UserID=@UserID "; 
            SqlParameter[] sp = new SqlParameter[11];
            sp[0] = new SqlParameter("@UserID", UserID);
            sp[1] = new SqlParameter("@DingDingUserID", DingDingUserID);
            sp[2] = new SqlParameter("@DingDingDeptID", DingDingDeptID);
            sp[3] = new SqlParameter("@UserName", UserName);
            sp[4] = new SqlParameter("@Account", Account);
            sp[5] = new SqlParameter("@Password", Password);
            sp[6] = new SqlParameter("@ChangeTime", ChangeTime);
            sp[7] = new SqlParameter("@Mobile", Mobile);
            sp[8] = new SqlParameter("@WorkPlace", WorkPlace);
            sp[9] = new SqlParameter("@Memo", Memo);
            sp[10] = new SqlParameter("@IsDeleted", IsDeleted);

         return dbHelper.ExcuteNonQueryForTransaction(strSQL,sp);
      }
      /// <summary>
      /// 获取删除数据的SQL语句
      /// <summary>
      public int SetDelete(string UserID)
      {
         string strSQL = "update [User] set IsDeleted=1, ChangeTime=getdate() WHERE UserID=@UserID  ";
            SqlParameter[] sp = new SqlParameter[1];
            sp[0] = new SqlParameter("@UserID", UserID);

         return new DbHelper().ExcuteNonQuery(strSQL,sp);
      }
      /// <summary>
      /// 获取删除数据的SQL语句
      /// <summary>
      public int SetDelete()
      {
         string strSQL = "update [User] set IsDeleted=1, ChangeTime=getdate() WHERE UserID=@UserID  ";
            SqlParameter[] sp = new SqlParameter[1];
            sp[0] = new SqlParameter("@UserID", UserID);

         return new DbHelper().ExcuteNonQuery(strSQL,sp);
      }
      /// <summary>
      /// 更新非空属性。
      /// <summary>
      public void UpdatePropFromNotNull(User user_)
      {
            if (!String.IsNullOrEmpty(user_.dingdinguserid_)  )
            {
               this.dingdinguserid_ = user_.DingDingUserID;
            }
            if (!String.IsNullOrEmpty(user_.dingdingdeptid_)  )
            {
               this.dingdingdeptid_ = user_.DingDingDeptID;
            }
            if (!String.IsNullOrEmpty(user_.username_)  )
            {
               this.username_ = user_.UserName;
            }
            if (!String.IsNullOrEmpty(user_.account_)  )
            {
               this.account_ = user_.Account;
            }
            if (!String.IsNullOrEmpty(user_.password_)  )
            {
               this.password_ = user_.Password;
            }
            if (!String.IsNullOrEmpty(user_.changetime_)  )
            {
               this.changetime_ = user_.ChangeTime;
            }
            if (!String.IsNullOrEmpty(user_.mobile_)  )
            {
               this.mobile_ = user_.Mobile;
            }
            if (!String.IsNullOrEmpty(user_.workplace_)  )
            {
               this.workplace_ = user_.WorkPlace;
            }
            if (!String.IsNullOrEmpty(user_.memo_)  )
            {
               this.memo_ = user_.Memo;
            }
            if (!String.IsNullOrEmpty(user_.isdeleted_)  )
            {
               this.isdeleted_ = user_.IsDeleted;
            }

      }
      /// <summary>
      /// 获取主键的值
      /// <summary>
      public override string GetKeyValue()
      {
         return ESC(userid_) ;
      }
      #endregion SetSQL Function
      #region GetObject Function
      /// <summary>
      /// 获取User数据
      /// <summary>
      public List<User> GetUserList(int Page, int Size)
      {
         List<User> users = new List<User>();
         string strSQL = " select * from "
         + " (select *,row_number() over (order by [User].UserID) as t "
         + " from [User]  where IsDeleted=0  ) as o where o.t between @Begin and @End"; 
         SqlParameter[] sp = new SqlParameter[2];
         sp[0] = new SqlParameter("@Begin", (Page - 1) * Size + 1);
         sp[1] = new SqlParameter("@End", Page * Size);
         using (SqlDataReader dr = new DbHelper().ExcuteReader(strSQL, sp)){
             while (dr.Read()){
             User user = new  User();
             user.UserID = dr["UserID"] == DBNull.Value ? "" : dr["UserID"].ToString();
             user.DingDingUserID = dr["DingDingUserID"] == DBNull.Value ? "" : dr["DingDingUserID"].ToString();
             user.DingDingDeptID = dr["DingDingDeptID"] == DBNull.Value ? "" : dr["DingDingDeptID"].ToString();
             user.UserName = dr["UserName"] == DBNull.Value ? "" : dr["UserName"].ToString();
             user.Account = dr["Account"] == DBNull.Value ? "" : dr["Account"].ToString();
             user.Password = dr["Password"] == DBNull.Value ? "" : dr["Password"].ToString();
             user.ChangeTime = dr["ChangeTime"] == DBNull.Value ? "" : dr["ChangeTime"].ToString();
             user.Mobile = dr["Mobile"] == DBNull.Value ? "" : dr["Mobile"].ToString();
             user.WorkPlace = dr["WorkPlace"] == DBNull.Value ? "" : dr["WorkPlace"].ToString();
             user.Memo = dr["Memo"] == DBNull.Value ? "" : dr["Memo"].ToString();
             user.IsDeleted = dr["IsDeleted"] == DBNull.Value ? "" : dr["IsDeleted"].ToString();

             users.Add(user);}}
         return users;
      }
      /// <summary>
      /// 获取User总数
      /// <summary>
      public int GetUsersCount()
      {
         string strSQL = " select count(*) from  [User]  where IsDeleted=0  "; 
         using (SqlDataReader dr = new DbHelper().ExcuteReader(strSQL)){
              if (dr.Read()){
              return dr[0] == DBNull.Value ? 0 : Convert.ToInt32(dr[0].ToString());}
              else return -1;}
      }
      /// <summary>
      /// 获取User数据
      /// <summary>
      public List<User> GetUserList()
      {
         List<User> users = new List<User>();
         string strSQL = " select * from [User]";
         using (SqlDataReader dr = new DbHelper().ExcuteReader(strSQL)){
             while (dr.Read()){
             User user = new  User();
             user.UserID = dr["UserID"] == DBNull.Value ? "" : dr["UserID"].ToString();
             user.DingDingUserID = dr["DingDingUserID"] == DBNull.Value ? "" : dr["DingDingUserID"].ToString();
             user.DingDingDeptID = dr["DingDingDeptID"] == DBNull.Value ? "" : dr["DingDingDeptID"].ToString();
             user.UserName = dr["UserName"] == DBNull.Value ? "" : dr["UserName"].ToString();
             user.Account = dr["Account"] == DBNull.Value ? "" : dr["Account"].ToString();
             user.Password = dr["Password"] == DBNull.Value ? "" : dr["Password"].ToString();
             user.ChangeTime = dr["ChangeTime"] == DBNull.Value ? "" : dr["ChangeTime"].ToString();
             user.Mobile = dr["Mobile"] == DBNull.Value ? "" : dr["Mobile"].ToString();
             user.WorkPlace = dr["WorkPlace"] == DBNull.Value ? "" : dr["WorkPlace"].ToString();
             user.Memo = dr["Memo"] == DBNull.Value ? "" : dr["Memo"].ToString();
             user.IsDeleted = dr["IsDeleted"] == DBNull.Value ? "" : dr["IsDeleted"].ToString();

             users.Add(user);}}
         return users;
      }
      /// <summary>
      /// 获取User数据 
      /// <summary>
      public List<User> GetUserListNotDeleted()
      {
         List<User> users = new List<User>();
         string strSQL = " select * from [User] Where IsDeleted=0";
         using (SqlDataReader dr = new DbHelper().ExcuteReader(strSQL)){
             while (dr.Read()){
             User user = new  User();
             user.UserID = dr["UserID"] == DBNull.Value ? "" : dr["UserID"].ToString();
             user.DingDingUserID = dr["DingDingUserID"] == DBNull.Value ? "" : dr["DingDingUserID"].ToString();
             user.DingDingDeptID = dr["DingDingDeptID"] == DBNull.Value ? "" : dr["DingDingDeptID"].ToString();
             user.UserName = dr["UserName"] == DBNull.Value ? "" : dr["UserName"].ToString();
             user.Account = dr["Account"] == DBNull.Value ? "" : dr["Account"].ToString();
             user.Password = dr["Password"] == DBNull.Value ? "" : dr["Password"].ToString();
             user.ChangeTime = dr["ChangeTime"] == DBNull.Value ? "" : dr["ChangeTime"].ToString();
             user.Mobile = dr["Mobile"] == DBNull.Value ? "" : dr["Mobile"].ToString();
             user.WorkPlace = dr["WorkPlace"] == DBNull.Value ? "" : dr["WorkPlace"].ToString();
             user.Memo = dr["Memo"] == DBNull.Value ? "" : dr["Memo"].ToString();
             user.IsDeleted = dr["IsDeleted"] == DBNull.Value ? "" : dr["IsDeleted"].ToString();

             users.Add(user);}}
         return users;
      }
      #endregion GetObject Function
   }
}
