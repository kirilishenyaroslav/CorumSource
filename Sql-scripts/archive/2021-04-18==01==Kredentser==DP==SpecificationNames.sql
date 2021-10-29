
update [Test].dbo.SpecificationNames
set nmcTestId = [Test].dbo.SpecificationNamess.nmcTestId, nmcWorkId = [Test].dbo.SpecificationNamess.nmcWorkId,
SpecCode = [Test].dbo.SpecificationNamess.SpecCode, SpecName = [Test].dbo.SpecificationNamess.SpecName,
industryId = [Test].dbo.SpecificationNamess.industryId, industryId_Test = [Test].dbo.SpecificationNamess.industryId_Test
from [Test].dbo.SpecificationNamess join [Test].dbo.SpecificationNames on [Test].dbo.SpecificationNamess.Id = [Test].dbo.SpecificationNames.Id
go

select*from [Test].dbo.SpecificationNames;
