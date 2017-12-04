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

        //Cada 5 minutos
        //Quartz.CronTrigger trigger1 = new Quartz.CronTrigger("myTriggerJob", null, "myTrigger", null, DateTime.UtcNow, null, "0 0/5 * * * ?");

        Quartz.CronTrigger trigger1 = new Quartz.CronTrigger("myTriggerJob", null, "myTrigger", null, DateTime.UtcNow, null, "0/10 * * * * ?");
        
        // schedule the job
        sched.ScheduleJob(job, trigger1);
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Código que se ejecuta cuando se cierra la aplicación
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Código que se ejecuta al producirse un error no controlado

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
