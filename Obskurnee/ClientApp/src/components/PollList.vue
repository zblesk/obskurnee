<template>
    <h1 id="tableLabel">Aktuálne hlasovania</h1>

    <p v-if="!polls"><em>Cakaj, nacitavam</em></p>

    <table class='table table-striped' aria-labelledby="tableLabel" v-if="polls">
        <tbody>
            <tr v-for="poll in polls" v-bind:key="poll"> 
                <td><router-link :to="{ name: 'poll', params: { pollId: poll.pollId } }">{{ poll.title }}</router-link></td>
            </tr>
        </tbody>
    </table>
</template>


<script>
    import axios from 'axios'
    export default {
        name: "Polls",
        data() {
            return {
                polls: [],
            }
        },
        methods: {
            getPolls() {
                axios.get('/api/polls')
                    .then((response) => {
                        this.polls =  response.data;
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
        },
        mounted() {
            this.getPolls();
        }
    }
</script>