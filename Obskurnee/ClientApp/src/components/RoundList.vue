<template>
<section>
<h1 id="tableLabel">Kolá návrhov</h1>

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
import { mapActions, mapState } from "vuex";
export default {
    name: "RoundList",
    computed: {
        ...mapState("rounds", ["rounds"]),
    },
    methods: {
        ...mapActions("rounds", ["fetchRounds"]),
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