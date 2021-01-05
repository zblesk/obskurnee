<template>
    <h1 id="tableLabel">Aktualne diskusie</h1>

    <p v-if="!discussions"><em>Cakaj, nacitavam</em></p>

    <table class='table table-striped' aria-labelledby="tableLabel" v-if="discussions">
        <tbody>
            <tr v-for="discussion of discussions" v-bind:key="discussion"> 
                <td><router-link :to="{ name: 'discussion', params: { id: discussion.id } }">{{ discussion.title }}</router-link></td>
            </tr>
        </tbody>
        <tfoot>
            <tr><td>
            <input v-model="newDiscussion.title" placeholder="Nova diskusia" />
            <button @click="postNewDiscussion">Pridaj</button></td></tr>
        </tfoot>
    </table>
</template>


<script>
    import axios from 'axios'
    export default {
        name: "Discussions",
        data() {
            return {
                discussions: [],
                newDiscussion: {},
            }
        },
        methods: {
            getDiscussions() {
                axios.get('/api/discussions')
                    .then((response) => {
                        this.discussions =  response.data;
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            postNewDiscussion() {
                console.log(this.newDiscussion);
                axios.post('/api/discussions/', this.newDiscussion)
                    .then((response) => {
                        this.discussions.push(response.data);
                        this.newDiscussion = {};
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            }
        },
        mounted() {
            this.getDiscussions();
        }
    }
</script>