CREATE PROCEDURE [dbo].[GetComponentsPageByComputerID]
	@ID smallint
WITH ENCRYPTION
AS
SELECT comps.[ComputerName],ct.[ComponentName],cs.[ComponentState],c.[Version],cset.[Name]  
		FROM Components AS c 
			INNER JOIN ComponentStates as cs ON c.[ComponentID] = cs.[ID]
			INNER JOIN Computers as comps ON comps.[ID] = c.[ComputerID]
			INNER JOIN ComponentSettings AS cset ON cset.[ID] = c.[SettingsID]
			INNER JOIN ComponentTypes AS ct ON ct.[ID] = c.[ComponentID]
		 WHERE c.[ComputerID]=@ID;