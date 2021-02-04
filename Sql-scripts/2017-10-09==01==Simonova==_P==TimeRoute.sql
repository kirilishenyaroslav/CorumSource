 update [dbo].[OrdersBase]
 set TimeRoute = TimeRoute / 3600000
 where (TimeRoute is not null)
  and (TimeRoute > 0)
  go