"use strict";

import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export default {
    store: null,
    notifier: null,
    install (Vue) 
    {
        const connection = new HubConnectionBuilder()
            .withUrl("/hubs/events")
            .configureLogging(LogLevel.Information)
            .build();
        this.store = Vue.config.globalProperties.$store;
        this.notifier = Vue.config.globalProperties.$toast;
        let startedPromise = null;
        function start () {
            startedPromise = connection.start().catch(err => {
            console.error('Failed to connect with hub', err)
            return new Promise((resolve, reject) => 
                setTimeout(() => start().then(resolve).catch(reject), 5000))
            })
            return startedPromise
        }
        connection.onclose(() => start());
        
        connection.on('DiscussionChanged', (data) => {
            this.onDiscussionChanged(data);
        })

        start();
    },
    onDiscussionChanged(discussion) 
    {
        this.store.commit("global/setCurrentDiscussion", discussion);
    }
}