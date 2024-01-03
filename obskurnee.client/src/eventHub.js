import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export default {
    store: null,
    notifier: null,
    connection: null,
    startedPromise: null,
    manuallyClosed: false,
    install (Vue) 
    {
        this.store = Vue.config.globalProperties.$store;
        this.notifier = Vue.config.globalProperties.$toast;


        this.store.subscribeAction({
            after: (action) => {
                
              // open hub connection on login or context restore
              if ((action.type == 'context/restoreContext'
                || action.type == 'context/login')
                && this.store.getters['context/isAuthenticated'])
              {
                this.startSignalR(this.store.state.context.jwtToken);
              }

              // close connection on logout
              if (action.type == 'context/logout')
              {
                this.stopSignalR();
              }
            }
        });        
    },
    onDiscussionChanged(discussion) 
    {
        this.store.commit("global/setCurrentDiscussion", discussion);
    },
    startSignalR(jwtToken) 
    {
        this.connection = new HubConnectionBuilder()
            .withUrl(
                "/hubs/events", 
                jwtToken ? { accessTokenFactory: () => jwtToken } : null)
            .configureLogging(LogLevel.Information)
            .build();
       
        this.connection.on('DiscussionChanged', (data) => {
            this.onDiscussionChanged(data);
        })
       
        this.connection.onclose(() => {
          if (!this.manuallyClosed) 
          {
              this.start();
          }
        })
       
        // Start now
        this.manuallyClosed = false;
        this.start();
    },
    start() {
        this.startedPromise = this.connection.start()
            .catch(err => {
                console.error('Failed to connect with hub', err);
                return new Promise((resolve, reject) => 
                    setTimeout(
                        () => this.start()
                            .then(resolve)
                            .catch(reject), 
                        5000))
            });
        return this.startedPromise;
    },
    stopSignalR() {
        if (!this.startedPromise) {
            return;
        }
     
        this.manuallyClosed = true;
        return this.startedPromise
            .then(() => this.connection.stop())
            .then(() => { this.startedPromise = null });
    }
}