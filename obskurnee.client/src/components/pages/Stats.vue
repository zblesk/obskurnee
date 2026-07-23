<template>
    <section>
        <h1 class="page-title">{{ $t('menus.stats') }}</h1>
        <div class="main">
            <h2 class="chart-title">{{ $t('stats.booksRated') }}</h2>
            <Bar v-if="mostRead" :data="mostRead" :options="chartOptions" />
        </div>
        <div class="main">
            <h2 class="chart-title">{{ $t('stats.recommendations') }}</h2>
            <Bar v-if="mostRecommended" :data="mostRecommended" :options="chartOptions" />
        </div>
        <div class="main" v-if="mostWins">
            <h2 class="chart-title">{{ $t('stats.winningBooks') }}</h2>
            <Bar :data="mostWins" :options="chartOptions" />
        </div>
    </section>
</template>

<script>
import { mapActions, mapState } from "vuex";
import { Bar } from "vue-chartjs";
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from "chart.js";

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

export default {
    name: "Stats",
    components: { Bar },
    computed: {
        ...mapState("stats", ["userStats"]),
        mostRead() {
            if (!this.userStats || !this.userStats.length) return null;
            const sorted = [...this.userStats].sort((a, b) => b.booksRatedCount - a.booksRatedCount);
            return {
                labels: sorted.map(u => u.userName),
                datasets: [{
                    label: this.$t('stats.booksRated'),
                    data: sorted.map(u => u.booksRatedCount),
                    backgroundColor: this.accentColor,
                }],
            };
        },
        mostRecommended() {
            if (!this.userStats || !this.userStats.length) return null;
            const sorted = [...this.userStats].sort((a, b) => b.recommendationCount - a.recommendationCount);
            return {
                labels: sorted.map(u => u.userName),
                datasets: [{
                    label: this.$t('stats.recommendations'),
                    data: sorted.map(u => u.recommendationCount),
                    backgroundColor: this.accentColor,
                }],
            };
        },
        mostWins() {
            if (!this.userStats || !this.userStats.length) return null;
            const filtered = [...this.userStats]
                .filter(u => u.winningBooksCount > 0)
                .sort((a, b) => b.winningBooksCount - a.winningBooksCount);
            if (!filtered.length) return null;
            return {
                labels: filtered.map(u => u.userName),
                datasets: [{
                    label: this.$t('stats.winningBooks'),
                    data: filtered.map(u => u.winningBooksCount),
                    backgroundColor: this.accentColor,
                }],
            };
        },
        accentColor() {
            return getComputedStyle(document.documentElement).getPropertyValue('--c-accent').trim();
        },
        chartOptions() {
            return {
                indexAxis: 'y',
                responsive: true,
                plugins: {
                    legend: { display: false },
                },
                scales: {
                    x: { beginAtZero: true, ticks: { stepSize: 1 } },
                },
            };
        },
    },
    methods: {
        ...mapActions("stats", ["fetchUserStats"]),
    },
    mounted() {
        this.fetchUserStats();
    },
    activated() {
        this.fetchUserStats();
    },
}
</script>

<style scoped>
    .chart-title {
        margin-top: 0;
        margin-bottom: calc(var(--spacer) * 2);
        text-align: center;
    }
</style>