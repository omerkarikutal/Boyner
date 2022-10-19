# Boyner
Boyner Case
Katmanlı mimari yapısı kullanıldı
cqrs yapısı kullanıldı
data lar olabildiğince cache üzerinde güncel tutuldu
entitiyframework codefirst yaklaşımı kullanıldı
auto db create edildi
seed metoduyla fake datalar implemente edildi
2 adet dal katmanı metodunun unit testi yazıldı (api , business logic lerde eklenebilir testlere)
ilişkisel bir yapı kullanıldı
transaction yapısı için unitofwork kullanıldı
mssql , redis , api docker container üzerine ayaklandırıldı..
Db yapısı olarak bir attribute tablosu var , bu attribute lere ait 1-n ilişkisi olan attributevalue tablosu var
Product tablosu ve Product Category arasında bir ilişki var 
Product lara ait attributevalue'lar var  , bunlar arasında ki ilişki n-n yapıdadır
Aynı yapısı ProductCategory içinde geçerlidir , attributevalue'lar ile n-n bir ilişki bulunmaktadır
transaction yapısı içerisinde outbox pattern yapısıda eklenebilir 

