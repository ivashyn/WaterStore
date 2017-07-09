use StoreDB
select m.Name, count(o.Id) as Orders
from Managers m
left join Orders o on o.ManagerId = m.Id
Where o.OrderDate Between '1.1.2017' and '12.27.2017'
or o.OrderDate is null
Group by m.Id, m.Name