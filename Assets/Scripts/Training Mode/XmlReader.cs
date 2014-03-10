using System;
using System.Xml;
using System.IO;
using ETraining;

/**
 * This class controls reading the content of xml scenario file and then initilize all the objects representing the Object Model
 * of project (\ref EngineAnimation, \ref Step, \ref Tool, \ref SingleAnimation ...)
 * It's based on XML reading function from C#. Further refer it to any resource from MSDN, etc.
 */ 

public class XmlReader
{
	private Step [] allStep;
	private int length;

	public int Length {
		get {
			return this.length;
		}
		set {
			length = value;
		}
	}
	public Step[] AllStep {
		get {
			return this.allStep;
		}
		set {
			allStep = value;
		}
	}

    public void loadXML(string filePath)
    {
        XmlDocument xmlDoc = new XmlDocument();
		try {
			xmlDoc.Load(filePath);
		} catch (Exception ex) {
			ErrorLog loadXMLLog = ErrorLogManager.createErrorLog();
			loadXMLLog.TypeLog = 1;
			loadXMLLog.ContentLog = "Could find file '"+ filePath +"'.";
			ErrorLogManager.addErrorLog(loadXMLLog);
			ErrorLogManager.errorFlag = true;
			return;
		}

//		using (Stream s = File.OpenRead(filePath))
//		{
//			xmlDoc.Load(s);
//		}
        // get all steps
        XmlNodeList stepList = xmlDoc.GetElementsByTagName("step"); // array of the level nodes.

        allStep = new Step[stepList.Count];
		length = stepList.Count;
        int stepsIndex = -1;

        foreach (XmlNode step in stepList)
        {
            string name = step.Attributes["name"].Value;
            int id = Int32.Parse(step.Attributes["id"].Value);
            allStep[++stepsIndex] = new Step( name, id);
            //steps[++stepsIndex].
			int numberOftask = 0;
            XmlNodeList stepInfoList = step.ChildNodes;
			
			AbstractCommonTask [] listOfTask = new AbstractCommonTask[30];
			
            foreach (XmlNode stepInfo in stepInfoList) // levels itens nodes.
            {
                if (stepInfo.Name == "image")
                {
                    allStep[stepsIndex].ImagePath = stepInfo.InnerText;
                }
                else if (stepInfo.Name == "video")
                {
                    allStep[stepsIndex].VideoPath = stepInfo.InnerText;
                }
				else if (stepInfo.Name == "linkedInfo")
                {
                    allStep[stepsIndex].LinkedInfo = stepInfo.InnerText;
                }
                else if (stepInfo.Name == "task")
                {
					numberOftask++;
					
					AbstractCommonTask task = new AbstractCommonTask();
                    string groupId = stepInfo.Attributes["groupid"].Value;
                    int taskId = Int32.Parse(stepInfo.Attributes["id"].Value);
                    string taskName =  stepInfo.Attributes["name"].Value;
					
                    string taskDescription = "", componentId = "", componentName = "", taskImage = "";
					bool hideAfterDisassembled = false;
					string toolModelName = "";
                    Tool[] listTools = null;
                    EngineAnimation[] listAnimations = null;

                    XmlNodeList taskInfoList = stepInfo.ChildNodes;
                    foreach (XmlNode taskInfo in taskInfoList)
                    {
						if(taskInfo.Name =="taskImage")
							taskImage = taskInfo.InnerText;
						
                        if (taskInfo.Name == "description")
                            taskDescription = taskInfo.InnerText;                             

                        if (taskInfo.Name == "component")
                        {
                            componentId = taskInfo.Attributes["id"].Value;
                            componentName = taskInfo.Attributes["name"].Value;
							if(taskInfo.Attributes["hideAfterDisassembled"] != null)
								hideAfterDisassembled = Boolean.Parse(taskInfo.Attributes["hideAfterDisassembled"].Value);							
                        }
                        if (taskInfo.Name == "tools")
                        {
							toolModelName =	taskInfo.Attributes["modelName"].Value;
                            XmlNodeList toolList = taskInfo.ChildNodes;
                            listTools = new Tool[toolList.Count];
                            int count =-1;
                            foreach (XmlNode tool in toolList)
                            {
                                listTools[++count] = new Tool(tool.Attributes["id"].Value, tool.Attributes["name"].Value);
                            }
                        }

                        if (taskInfo.Name == "animations")
                        {
                            XmlNodeList animationList = taskInfo.ChildNodes;
                            int count = -1;
                            listAnimations = new EngineAnimation[animationList.Count];
                            if(animationList.Count == 1) 
                            {
                                string animationName = animationList.Item(0).InnerText;
								int run = int.Parse(animationList.Item(0).Attributes["runOrder"].Value);
								int start = int.Parse(animationList.Item(0).Attributes["startOrder"].Value);
								int end = int.Parse(animationList.Item(0).Attributes["endOrder"].Value);

								try {
									EngineAnimation temp = new EngineAnimation(animationName, run, start, end);								
									task = new SingleAnimationTask(temp); 
								} catch (Exception ex) {
									ErrorLog errorLog = ErrorLogManager.createErrorLog();
									errorLog.TypeLog = 2;
									errorLog.ContentLog = "Could not find animation '"+ animationName +"'";
									ErrorLogManager.addErrorLog(errorLog);
									ErrorLogManager.errorFlag = true;
								}

								                                 																
                            }
                            else 
                            {                                  
                                foreach (XmlNode animation in animationList)
                                {
									
									string animationName = animation.InnerText;
									int run = int.Parse(animationList.Item(0).Attributes["runOrder"].Value);
									int start = int.Parse(animationList.Item(0).Attributes["startOrder"].Value);
									int end = int.Parse(animationList.Item(0).Attributes["endOrder"].Value);								

									try {
										listAnimations[++count] = new EngineAnimation(animationName, run, start, end);
									} catch (Exception ex) {
										ErrorLog errorLog = ErrorLogManager.createErrorLog();
										errorLog.TypeLog = 2;
										errorLog.ContentLog = "Could not find animation '"+ animationName +"'";
										ErrorLogManager.addErrorLog(errorLog);
										ErrorLogManager.errorFlag = true;
									}

						            
                                }
                                //task = new MultipleAnimationTask(listAnimations);
                            }                                 
                        }                           
                    }

                    task.Id = taskId;
					task.ToolModelName = toolModelName;
                    task.GroupId = groupId;
                    task.TaskName = taskName;
                    task.Description = taskDescription;
					task.ImagePath = taskImage;
                    task.ListOfTools = listTools;
					try {
						task.Component = new EngineComponent(componentName, componentId, hideAfterDisassembled);	
					} catch (Exception ex) {
						ErrorLog errorLog = ErrorLogManager.createErrorLog();
						errorLog.TypeLog = 2;
						errorLog.ContentLog = "Could not find component '"+ componentName +"'";
						ErrorLogManager.addErrorLog(errorLog);
						ErrorLogManager.errorFlag = true;
					}

                    // step

                    listOfTask[numberOftask -1] = task;
                }
            }
			allStep[stepsIndex].ListOfTask = new AbstractCommonTask[numberOftask];
			for (int i = 0; i < numberOftask; i++) {
				allStep[stepsIndex].ListOfTask[i] = listOfTask[i];
			}
			 
        }
        int debughere = 0;

    }
}

