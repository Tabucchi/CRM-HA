<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Código que se ejecuta al iniciarse la aplicación
        NameValueCollection properties = new NameValueCollection();
        properties["quartz.scheduler.instanceName"] = "RemoteServer";

        // set thread pool info
        properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
        properties["quartz.threadPool.threadCount"] = "1";
        properties["quartz.threadPool.threadPriority"] = "Normal";

        Quartz.ISchedulerFactory sf = new Quartz.Impl.StdSchedulerFactory(properties);
        Quartz.IScheduler sched = sf.GetScheduler();
        sched.Start();

        // define the job and ask it to run
        Quartz.JobDetail job = new Quartz.JobDetail("myTrigger", null, typeof(JobScheduler));
        Quartz.JobDataMap map = new Quartz.JobDataMap();
        map.Put("msg", "Your remotely added job has executed!");
        job.JobDataMap = map;

        //A las 10:15 am del día 15 de cada mes
        //Quartz.CronTrigger trigger1 = new Quartz.CronTrigger("myTriggerJob", null, "myTrigger", null, DateTime.UtcNow, null, "0 15 10 01 * ?");
        //Quartz.CronTrigger trigger1 = new Quartz.CronTrigger("myTriggerJob", null, "myTrigger", null, DateTime.UtcNow, null, "0 06 15 02 * ?");
        Quartz.CronTrigger trigger1 = new Quartz.CronTrigger("myTriggerJob", null, "myTrigger", null, DateTime.UtcNow, null, "0 58 15 * * ? *");
                       
        // schedule the job
        sched.ScheduleJob(job, trigger1);

        
        // define the job and ask it to run
        Quartz.JobDetail jobIndice = new Quartz.JobDetail("myTrigger1", null, typeof(JobSchedulerCuotas));
        Quartz.JobDataMap mapIndice = new Quartz.JobDataMap();
        mapIndice.Put("msg", "Your remotely added job has executed!");
        jobIndice.JobDataMap = mapIndice;

        //A las 10:15 am del día 15 de cada mes
        //Quartz.CronTrigger trigger1 = new Quartz.CronTrigger("myTriggerJob", null, "myTrigger", null, DateTime.UtcNow, null, "0 15 10 01 * ?");
        //Quartz.CronTrigger trigger1 = new Quartz.CronTrigger("myTriggerJob", null, "myTrigger", null, DateTime.UtcNow, null, "0 06 15 02 * ?");
        Quartz.CronTrigger triggerIndice = new Quartz.CronTrigger("myTriggerJobIndice", null, "myTrigger1", null, DateTime.UtcNow, null, "0 40 08 * * ? *");

        // schedule the job
        sched.ScheduleJob(jobIndice, triggerIndice);
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Código que se ejecuta cuando se cierra la aplicación
    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        Server.Transfer("MensajeError.aspx");
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Código que se ejecuta cuando se inicia una nueva sesión
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Código que se ejecuta cuando finaliza una sesión. 
        // Nota: el evento Session_End se desencadena sólo cuando el modo sessionstate
        // se establece como InProc en el archivo Web.config. Si el modo de sesión se establece como StateServer 
        // o SQLServer, el evento no se genera.
    }
       
</script>
