<template>
<section>
<h1 id="tableLabel">Kolá návrhov</h1>

<div v-if="isMod">
    Začať s: 
    <input type="radio" id="Books" value="Books" v-model="newRound.topic"> Knihy
    <input type="radio" id="Themes" value="Themes" v-model="newRound.topic"> Témy
    <input v-model="newRound.title" placeholder="Názov (napr. 'Kniha IXY')" required />
    <textarea v-model="newRound.description" placeholder="Popis / zadanie"></textarea>
    <button @click="cNewRound">Nové kolo</button>
</div>

<p v-if="!rounds"><em>Cakaj, nacitavam</em></p>
<div v-for="round of rounds" v-bind:key="round" style="margin:auto;"> 
    <h3>{{ round.title }}</h3>
    <table>
        <tr>
            <td><router-link v-if="round.themeDiscussionId" :to="{ name: 'discussion', params: { discussionId: round.themeDiscussionId } }">Návrhy tém</router-link></td>
            <td>
                <router-link v-if="round.themePollId" :to="{ name: 'poll', params: { pollId: round.themePollId } }">Hlasovanie o témach</router-link> 
            </td>
        </tr><tr>
            <td><router-link v-if="round.bookDiscussionId" :to="{ name: 'discussion', params: { discussionId: round.bookDiscussionId } }">Návrhy kníh</router-link> </td>
            <td><router-link v-if="round.bookPollId" :to="{ name: 'poll', params: { pollId: round.bookPollId } }">Hlasovanie o knihách</router-link> </td>
        </tr>
        <tr>
            <td><span v-if="round.bookId"> este link na knihu s ID #{{ round.bookId}}</span></td>
        </tr>
    </table>
</div>
</section>
</template>


<script>
import { mapActions, mapState, mapGetters } from "vuex";
export default {
    name: "RoundList",
    data() { 
        return {
            newRound: {
                topic: "Books"
            }
        }
    },
    computed: {
        ...mapState("rounds", ["rounds"]),
        ...mapGetters("context", ["isMod"])
    },
    methods: {
        ...mapActions("rounds", ["fetchRounds", "createNewRound"]),
        cNewRound()
        {
            this.createNewRound(this.newRound).then(() => 
            {
                this.newRound = {};
            });
        }
    },
    mounted() {
        this.fetchRounds();
    }
}
</script>

<style scoped>
.archived a {
    color: hotpink;
}

td {
    border-color: pink;
    padding: 1em;
}
</style>